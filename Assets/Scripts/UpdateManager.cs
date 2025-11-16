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
    List<IUpdatable> toAdd = new List<IUpdatable>();
    List<IUpdatable> toRemove = new List<IUpdatable>();

    List<IFixedUpdatable> fixedUpdatables = new List<IFixedUpdatable>();
    List<IFixedUpdatable> fixedToAdd = new List<IFixedUpdatable>();
    List<IFixedUpdatable> fixedToRemove = new List<IFixedUpdatable>();

    private void Update()
    {
        ApplyPendingChanges();

        for (int i = 0; i < updatables.Count; i++)
        {
            updatables[i].OnUpdate();
        }
    }

    private void FixedUpdate()
    {
        ApplyPendingFixedChanges();

        for (int i = 0; i < fixedUpdatables.Count; i++)
        {
            fixedUpdatables[i].OnFixedUpdate();
        }
    }

    void ApplyPendingChanges()
    {
        if (toAdd.Count > 0)
        {
            updatables.AddRange(toAdd);
            toAdd.Clear();
        }

        if (toRemove.Count > 0)
        {
            foreach (var u in toRemove)
                updatables.Remove(u);
            toRemove.Clear();
        }
    }

    void ApplyPendingFixedChanges()
    {
        if (fixedToAdd.Count > 0)
        {
            fixedUpdatables.AddRange(fixedToAdd);
            fixedToAdd.Clear();
        }

        if (fixedToRemove.Count > 0)
        {
            foreach (var f in fixedToRemove)
                fixedUpdatables.Remove(f);
            fixedToRemove.Clear();
        }
    }

    public void AddUpdatable(IUpdatable updatable)
    {
        toAdd.Add(updatable);
    }

    public void RemoveUpdatable(IUpdatable updatable)
    {
        toRemove.Add(updatable);
    }

    public void AddFixedUpdatable(IFixedUpdatable fixedUpdatable)
    {
        fixedToAdd.Add(fixedUpdatable);
    }

    public void RemoveFixedUpdatable(IFixedUpdatable fixedUpdatable)
    {
        fixedToRemove.Add(fixedUpdatable);
    }
}
