namespace MoneyBook.Services {

    public struct Result {

        private bool isError;
        
        private string message;

        private Result(bool isError, string message = "") {
            this.isError = isError;
            this.message = message;
        }

        public bool IsSuccess => !isError;

        public bool IsError => isError;

        public string Message => message;

        public static Result Success(string message = "") {
            return new Result(false, message);
        }

        public static Result Error(string message = "") {
            return new Result(true, message);
        }
    }
}
