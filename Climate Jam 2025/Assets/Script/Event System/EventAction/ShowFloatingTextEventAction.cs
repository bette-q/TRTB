//using UnityEngine;

//[CreateAssetMenu(menuName = "EventActions/ShowFloatingTextEventAction")]
//public class ShowFloatingTextEventAction : EventAction
//{
//    public string inkKnot;

//    public override void Execute(Interactable caller)
//    {
//        var areaTrigger = caller as InteractAreaTrigger;
//        if (areaTrigger != null && !string.IsNullOrEmpty(inkKnot))
//        {
//            string content = InkManager.Instance.GetTextFromKnot(inkKnot);
//            areaTrigger.ShowFloatingText(content);
//        }
//    }
//}
