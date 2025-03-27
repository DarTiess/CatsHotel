using System.Collections.Generic;
using DefaultNamespace;
using GamePlayObjects.Cat.StateMachine;
using GamePlayObjects.Spawners;
using UnityEngine;

namespace GamePlayObjects.Fabrics
{
    public class TreatmentDepartFabric: IPlaceFabric
    {
        private List<TreatmentDepart> _treatmentSpawnerList;
        private Queue<StateTreatment> _catAtTreatment;
        private Vector3[] _waypoints;

        public TreatmentDepartFabric(List<TreatmentDepart> treatmentSpawners, SpawnerSetting settings ,
                                     List<HospitalTreatmentTable> hospitalTreatmentTables, SpawnerSetting hospitalTreatmentSettings,
                                     SpawnerSetting hospitalMedicamentSettings, List<Transform> waypoints)
        {
            _treatmentSpawnerList = treatmentSpawners;
            _catAtTreatment = new Queue<StateTreatment>();

            _waypoints = new Vector3[waypoints.Count];
            for (int i = 0; i < waypoints.Count; i++)
            {
                _waypoints[i] = waypoints[i].position;
            }
           

            InitializeSpawner(treatmentSpawners, settings);
            InitializeSpawner(hospitalTreatmentTables, hospitalTreatmentSettings);
            foreach (TreatmentDepart treatmentTable in treatmentSpawners)
            {
                treatmentTable.CatDepartToTakeTreatment += OnCatMakeTreatment;
            } 
            foreach (HospitalTreatmentTable treatmentTable in hospitalTreatmentTables)
            {
                treatmentTable.InitMedicament(hospitalMedicamentSettings);
                treatmentTable.FreeCat += OnFreeCat;
            }
        }

        private void OnFreeCat(Transform originPosition)
        {
            _catAtTreatment.Dequeue().MoveCatToExit(_waypoints,originPosition );
        }

        private void OnCatMakeTreatment(StateTreatment cat)
        {
            _catAtTreatment.Enqueue(cat);
        }

        private void InitializeSpawner<T>(List<T> spawnerList, SpawnerSetting settings) where T: SpawnSystem
        {
            foreach (T packSpawner in spawnerList)
            {
                packSpawner.Init(settings);
            }
        }
        public Transform GetPlacePosition()
        {
            int rnd = Random.Range(0, _treatmentSpawnerList.Count);
            Transform foodPos= _treatmentSpawnerList[rnd].transform;
          
            return foodPos;
        }
    }
}