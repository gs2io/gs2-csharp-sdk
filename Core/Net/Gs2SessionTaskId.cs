namespace Gs2.Core.Net
{
    public class Gs2SessionTaskId
    {
        private const int InvalidIdValue = 0;
        private const int ReservedIdValueMax = 10000;

        public readonly static Gs2SessionTaskId InvalidId = new(InvalidIdValue);

        private readonly int _value;

        private Gs2SessionTaskId(int value)
        {
            this._value = value;
        }

        public Gs2SessionTaskId(string value)
        {
            this._value = int.TryParse(value, out var num) ? num : InvalidIdValue;
        }

        public override string ToString()
        {
            return this._value.ToString();
        }
		
        public static bool operator ==(Gs2SessionTaskId gs2SessionTaskId1, Gs2SessionTaskId gs2SessionTaskId2)
        {
            return gs2SessionTaskId1?._value == gs2SessionTaskId2?._value;
        }

        public static bool operator !=(Gs2SessionTaskId gs2SessionTaskId1, Gs2SessionTaskId gs2SessionTaskId2)
        {
            return !(gs2SessionTaskId1 == gs2SessionTaskId2);
        }

        public override bool Equals(object obj)
        {
            if (obj is Gs2SessionTaskId id)
            {
                return this._value.Equals(id._value);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this._value.GetHashCode();
        }

        public static class Generator
        {
            private static int _valueCounter = InvalidIdValue;

            public static Gs2SessionTaskId Issue()
            {
                if (++_valueCounter <= ReservedIdValueMax)
                {
                    _valueCounter = ReservedIdValueMax + 1;
                }

                return new Gs2SessionTaskId(_valueCounter);
            }
        }
    }
}