namespace DhofarAppApi.Model
{
    public class OperationResult
    {
        public bool SuccessMessage { get; private set; }
        public string ErrorMessage { get; private set; }

        private OperationResult(bool success, string errorMessage)
        {

            SuccessMessage = success;
            ErrorMessage = errorMessage;
        }

        public static OperationResult Success()
        {
            return new OperationResult(true, null);
        }

        public static OperationResult Failed(string errorMessage)
        {
            return new OperationResult(false, errorMessage);
        }
    }

}
