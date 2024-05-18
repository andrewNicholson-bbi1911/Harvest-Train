namespace ResourceSystem
{
    [System.Serializable]
    public class IntResourceData : ResourceData<int>
    {


        public IntResourceData(ResourceSO resource, int value, bool isInfinite = false) : base(resource, value, isInfinite)
        {

        }


        public override ResourceData<int> CloneWithValue(int value)
        {
            return new IntResourceData(_resource, value, _isInfinitySource);
        }


        public bool TryChangeValue(int changeValue)
        {

            if (_isInfinitySource)
            {
                return true;
            }

            if(_value + changeValue >= 0)
            {
                _value += changeValue;
                return true;
            }

            

            return false;
        }

        public override int ValueFromString(string str)
        {
            int res = 0;
            if(int.TryParse(str, out res))
            {
                return res;
            }
            return int.Parse(str);
        }
    }
}

