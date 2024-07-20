using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Logging;
//using DG.Tweening;
public class ButtonWithTextUi : MonoBehaviour
{
    private VisualElement _bottomContatiner;
    private Button _open_btn;
    private Button _close_btn;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _bottomContatiner = root.Q<VisualElement>("Contatiner");
        _open_btn = root.Q<Button>("open_btn");
        _open_btn = root.Q<Button>("close_btn");
        //hide bottiom group
        _bottomContatiner.style.display = DisplayStyle.None;
        //event allocation 
        _open_btn.RegisterCallback<ClickEvent>(OnOpenButtonClick);
    }
    private void OnOpenButtonClick(ClickEvent evt)
    {
        //display bottom group
        _bottomContatiner.style.display = DisplayStyle.Flex;
        //add style (event)
        //_bottomContatiner.AddToClassList() 
        //_bottomContatiner.RemoveFromClassList
    }
    // Update is called once per frame
    void Update()
    {
        var _manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var finder = _manager.CreateEntityQuery(typeof(PlayerCountData));
        var root = finder.GetSingletonEntity();
        var origin = _manager.GetComponentData<PlayerCountData>(root);
        Log.Info(origin);
    }
    private void LoopingAnimation(TransitionEndEvent evt)
    {

        _bottomContatiner.ToggleInClassList("toogle_animation_style");
        _bottomContatiner.RegisterCallback<TransitionEndEvent>(evt => _bottomContatiner.ToggleInClassList("toogle_animation_style"));
    }
}


