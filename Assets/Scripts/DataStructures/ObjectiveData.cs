using UnityEngine;


public enum ObjectiveType
{
    MainObjective,
    SideObjective
}

public enum ObjectiveStatus
{
    Incomplete,
    Complete
}

public class ObjectiveData
{
    public ObjectiveType objectiveType;

    public string objectiveName;

    public string objectiveDescription;

    public ObjectiveStatus objectiveStatus;

    public bool isCall;

    public string callNode; // if there is a call to be made! (isCall == true)

}

