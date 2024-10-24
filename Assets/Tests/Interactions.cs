using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Interaction
{
   private enum InteractionType
   {
       None = 0,
       Click,
       DragAndDrop,
       Swipe,
   }


   private InteractionType type;
   private GameObject target;
   private Vector2? origin;
   private Vector2? destination;
   private Vector2? offset;


   public Interaction Click()
   {
       type = InteractionType.Click;
       return this;
   }


   public Interaction Drag(GameObject gameObject)
   {
       type = InteractionType.DragAndDrop;
       target = gameObject;
       return this;
   }


   public Interaction Swipe(GameObject gameObject)
   {
       type = InteractionType.Swipe;
       target = gameObject;
       return this;
   }


   public Interaction Button(Button button)
   {
       target = button.gameObject;
       RectTransform rectTransform = target.GetComponent<RectTransform>();
       origin = rectTransform.TransformPoint(rectTransform.rect.center);
       return this;
   }


   public Interaction From(Vector2 from)
   {
       origin = from;
       return this;
   }


   public Interaction To(Vector2 to)
   {
       destination = to;
       return this;
   }


   public Interaction Down(int magnitude)
   {
       offset = new Vector2(0, -magnitude);
       return this;
   }


   public Interaction Up(int magnitude)
   {
       offset = new Vector2(0, magnitude);
       return this;
   }


   public Interaction At(Vector2 orig)
   {
       origin = orig;
       return this;
   }
   
   public async Task Perform()
   {
       switch (type)
       {
           case InteractionType.Click:
               await PerformClick();
               break;
           case InteractionType.DragAndDrop:
               await PerformDragAndDrop();
               break;
           case InteractionType.Swipe:
               await PerformSwipe();
               break;
           default:
               throw new InvalidInteractionException("Invalid interaction type.");
       }
   }


   private async Task PerformClick()
   {
       PointerEventData point = new PointerEventData(EventSystem.current) { position = origin.Value };
       ExecuteEvents.ExecuteHierarchy(target, point, ExecuteEvents.pointerClickHandler);
       await Task.Delay(0);
   }


   private async Task PerformDragAndDrop()
   {
       float durationInSeconds = 0.5f;
       float frameDurationInSeconds = 1 / 60f;
       int frameDurationInMilliseconds = (int)(frameDurationInSeconds * 1000);


       PointerEventData point = new PointerEventData(EventSystem.current) { position = origin.Value };
       ExecuteEvents.ExecuteHierarchy(target, point, ExecuteEvents.beginDragHandler);
       await Task.Delay(0);


       Vector2 entireDelta = destination.Value - origin.Value;
       Vector2 delta = entireDelta / (durationInSeconds / frameDurationInSeconds);


       Vector2 oldPosition = origin.Value;
       for (float timePassed = 0f; timePassed < durationInSeconds; timePassed += frameDurationInSeconds)
       {
           Vector2 newPosition = oldPosition + delta;
           PointerEventData moveData = new PointerEventData(EventSystem.current)
           {
               position = newPosition,
               delta = delta
           };
           ExecuteEvents.ExecuteHierarchy(target, moveData, ExecuteEvents.dragHandler);
           await Task.Delay(frameDurationInMilliseconds);
           oldPosition = newPosition;
       }


       point = new PointerEventData(EventSystem.current) { position = destination.Value };
       ExecuteEvents.ExecuteHierarchy(target, point, ExecuteEvents.endDragHandler);
       await Task.Delay(0);
   }


   private async Task PerformSwipe()
   {
       if (destination.HasValue == false)
       {
           if (offset.HasValue)
           {
               destination = origin.Value + offset.Value;
           }
           else
           {
               throw new InvalidInteractionException("A destination is missing. Did you forget to call Down, Up, or To?");
           }
       }
       await PerformDragAndDrop();
   }
}


public class InvalidInteractionException : Exception
{
   public InvalidInteractionException() { }
   public InvalidInteractionException(string message) : base(message) { }
   public InvalidInteractionException(string message, Exception inner) : base(message, inner) { }
}

