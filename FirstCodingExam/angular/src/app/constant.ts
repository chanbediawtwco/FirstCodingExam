export abstract class Constants {
    public static readonly string = {
        Empty: ""
    };
    static readonly UserConfirmed = "CONFIRMED";
    static readonly Message = { 
        Delete: "Are you sure you want to delete?",
        RegistrationSuccessful: "Registration Successful"
    }
    static readonly Error = {
        LbirLargerThanUbir: "Lower bound interest rate is higher than Upper bound interest rate",
        AccessNotAllowed: "Access not allowed!",
        MissingInformation: "Required Information are missing",
        DifferentPasswordConfirmation: "Password is different to password confirmation"
    }

    static readonly Modal = {
        Medium: 'md',
        Large: 'lg'
    }
}