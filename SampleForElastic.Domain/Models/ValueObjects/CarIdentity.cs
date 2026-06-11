namespace SampleForElastic.Domain.Models.ValueObjects
{
    public class CarIdentity
    {
        public CarIdentity() { }
        public CarIdentity(string name, string code, string color)
        {
            CheckCarName(name);
            CheckCarCode(code);
            CheckCarColor(color);

            Name = name;
            Code = code;
            Color = color;
        }

        public string Name { get; private set; }
        public string Code { get; private set; }
        public string Color { get; private set; }


        public static CarIdentity Create(string name, string code, string color)
                        => new CarIdentity(name, code, color);

        public void UpdateAll(string name, string code, string color)
        {
            CheckCarName(name);
            CheckCarCode(code);
            CheckCarColor(color);

            this.Name = name;
            this.Code = code;
            this.Color = color;
        }


        public void UpdateCarName(string newName)
        {
            CheckCarName(newName);
            Name = newName;
        }
        public void UpdateCarCode(string newCode)
        {
            CheckCarCode(newCode);
            Code = newCode;
        }
        public void UpdateCarColor(string carColor)
        {
            CheckCarColor(carColor);
            Color = carColor;
        }

        #region Validations
        private void CheckCarName(string carName)
        {
            if (string.IsNullOrWhiteSpace(carName))
            {
                throw new Exception();
            }
        }
        private void CheckCarCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new Exception();
            }
        }
        private void CheckCarColor(string color)
        {
            if (string.IsNullOrWhiteSpace(color))
            {
                throw new Exception();
            }
        }
        #endregion


    }
}
