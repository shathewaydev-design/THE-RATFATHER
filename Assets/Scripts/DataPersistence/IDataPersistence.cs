using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public interface IDataPersistence
{
    void LoadData(GameData data);
    //when we load data, the implementing script should just read the data
    void SaveData(ref GameData data);
    //when we save data, we allow implementing script to modify the data
}
