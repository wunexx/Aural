using System.Collections.Generic;
using UnityEngine;

public interface IUpdatable 
{
    public void OnUpdate();
}
public interface IFixedUpdatable
{
    public void OnFixedUpdate();
}
public class UpdateManager : MonoBehaviour
{
    List<IUpdatable> updatables = new List<IUpdatable>();
    List<IFixedUpdatable> fixedUpdatables = new List<IFixedUpdatable>();

    private void Update()
    {
        foreach (var updatable in updatables)
        {
            updatable.OnUpdate();
        }
    }
    private void FixedUpdate()
    {
        foreach (var fixedUpdatable in fixedUpdatables)
        {
            fixedUpdatable.OnFixedUpdate();
        }
    }

    public void AddUpdatable(IUpdatable updatable)
    {
        updatables.Add(updatable);
    }
    public void RemoveUpdatable(IUpdatable updatable)
    {
        updatables.Remove(updatable);
    }
    public void AddFixedUpdatable(IFixedUpdatable fixedUpdatable)
    {
        fixedUpdatables.Add(fixedUpdatable);
    }
    public void RemoveFixedUpdatable(IFixedUpdatable fixedUpdatable)
    {
        fixedUpdatables.Remove(fixedUpdatable);
    }
}
