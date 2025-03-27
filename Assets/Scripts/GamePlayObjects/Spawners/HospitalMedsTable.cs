using GamePlayObjects.Fabrics;
using GamePlayObjects.Player;

namespace GamePlayObjects.Spawners
{
    public class HospitalMedsTable: SpawnSystem, IAddItem
    {
        private HospitalTreatmentTable _hospitalTable;

        public void AddItem()
        {
            _hospitalTable.PushCat();
        }

        protected override void SpawnItem()
        {
            
        }

        public void InitHospitalTreatmentTable(HospitalTreatmentTable hospitalTreatmentTable)
        {
            _hospitalTable = hospitalTreatmentTable;
        }
    }
}