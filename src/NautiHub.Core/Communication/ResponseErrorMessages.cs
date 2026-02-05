using Microsoft.Extensions.Localization;
using NautiHub.Core.Resources;

namespace NautiHub.Core.Communication;

public class ResponseErrorMessages
{
    private readonly MessagesService? _messagesService;

    public ResponseErrorMessages()
    {
        Mensages = [];
    }

    public ResponseErrorMessages(MessagesService messagesService)
    {
        _messagesService = messagesService;
        Mensages = [];
    }

    public List<string> Mensages { get; set; }

    public void AddErrorMessage(string messageKey)
    {
        // Se a chave corresponder a uma mensagem conhecida, usa a versão localizada
        var localizedMessage = GetLocalizedMessage(messageKey);
        Mensages.Add(localizedMessage);
    }

    public void AddErrorMessage(string messageKey, params object[] args)
    {
        var localizedMessage = GetLocalizedMessage(messageKey, args);
        Mensages.Add(localizedMessage);
    }

    private string GetLocalizedMessage(string messageKey, params object[] args)
    {
        if (_messagesService == null)
            return messageKey; // Retorna a chave original se não houver serviço de mensagens

         return messageKey switch
         {
             "Boat_Already_Exists" => _messagesService.Boat_Already_Exists,
             "Boat_Not_Found" => _messagesService.Boat_Not_Found,
             "BoatImage_Not_Found" => _messagesService.BoatImage_Not_Found,
             "Auth_User_Created" => _messagesService.Auth_User_Created,
             "Auth_User_Logged_In" => _messagesService.Auth_User_Logged_In,
             "Auth_Account_Locked" => _messagesService.Auth_Account_Locked,
             "Auth_Invalid_Credentials" => _messagesService.Auth_Invalid_Credentials,
             "Auth_Email_Password_Required" => _messagesService.Auth_Email_Password_Required,
             "Auth_Email_In_Use" => _messagesService.Auth_Email_In_Use,
             "Auth_User_Not_Found" => _messagesService.Auth_User_Not_Found,
             "Auth_Account_Not_Allowed" => _messagesService.Auth_Account_Not_Allowed,
             "Auth_Logout_Success" => _messagesService.Auth_Logout_Success,
             "Auth_Logout_Error" => _messagesService.Auth_Logout_Error,
             "Auth_Add_Role_Success" => _messagesService.Auth_Add_Role_Success,
             "Auth_Add_Role_Error" => _messagesService.Auth_Add_Role_Error,
             "Error_Internal_Server" => _messagesService.Error_Internal_Server,
             "Error_Registering_User" => _messagesService.Error_Registering_User,
             "Error_Login" => _messagesService.Error_Login,
              "Error_Internal_Server_Generic" => _messagesService.Error_Internal_Server_Generic,
              "Payment_Not_Found" => _messagesService.Payment_Not_Found,
              "Payment_Created_Success" => _messagesService.Payment_Created_Success,
              "Payment_Invalid_Method" => _messagesService.Payment_Invalid_Method,
              "Payment_Insufficient_Value" => _messagesService.Payment_Insufficient_Value,
              "Payment_Card_Expired" => _messagesService.Payment_Card_Expired,
              "Payment_Invalid_Document" => _messagesService.Payment_Invalid_Document,
              "Payment_Split_Invalid_Total" => _messagesService.Payment_Split_Invalid_Total,
              "Payment_Webhook_Invalid_Token" => _messagesService.Payment_Webhook_Invalid_Token,
              "Payment_Refund_Not_Allowed" => _messagesService.Payment_Refund_Not_Allowed,
              "Payment_Overdue_Notice" => _messagesService.Payment_Overdue_Notice,
              "Payment_Chargeback_Received" => _messagesService.Payment_Chargeback_Received,
              "Payment_QRCode_Not_Allowed" => _messagesService.Payment_QRCode_Not_Allowed,
              "Payment_No_Asaas_Id" => _messagesService.Payment_No_Asaas_Id,
              "Payment_Asaas_Error" => _messagesService.Payment_Asaas_Error,
              "Payment_General_Error" => _messagesService.Payment_General_Error,
              "Payment_Booking_Not_Found" => _messagesService.Payment_Booking_Not_Found,
              "Payment_List_Error" => _messagesService.Payment_List_Error,
               "Payment_Get_Status_Error" => _messagesService.Payment_Get_Status_Error,
               
               // Boat Messages
               "Boat_Name_Required" => _messagesService.Boat_Name_Required,
               "Boat_Name_Too_Long" => _messagesService.Boat_Name_Too_Long,
               "Boat_Description_Required" => _messagesService.Boat_Description_Required,
               "Boat_Description_Too_Long" => _messagesService.Boat_Description_Too_Long,
               "Boat_Document_Required" => _messagesService.Boat_Document_Required,
               "Boat_Document_Too_Long" => _messagesService.Boat_Document_Too_Long,
               "Boat_Capacity_Invalid" => _messagesService.Boat_Capacity_Invalid,
               "Boat_Capacity_Too_High" => _messagesService.Boat_Capacity_Too_High,
               "Boat_Price_Invalid" => _messagesService.Boat_Price_Invalid,
               "Boat_Price_Too_High" => _messagesService.Boat_Price_Too_High,
               "Boat_City_Required" => _messagesService.Boat_City_Required,
               "Boat_City_Too_Long" => _messagesService.Boat_City_Too_Long,
               "Boat_State_Required" => _messagesService.Boat_State_Required,
               "Boat_State_Invalid_Format" => _messagesService.Boat_State_Invalid_Format,
               "Boat_Only_Approved_Can_Be_Active" => _messagesService.Boat_Only_Approved_Can_Be_Active,
               "Boat_Already_Approved" => _messagesService.Boat_Already_Approved,
               "Boat_Cannot_Reject_Approved" => _messagesService.Boat_Cannot_Reject_Approved,
               "Boat_Only_Approved_Can_Be_Suspended" => _messagesService.Boat_Only_Approved_Can_Be_Suspended,
               "Boat_Only_Suspended_Can_Be_Reactived" => _messagesService.Boat_Only_Suspended_Can_Be_Reactived,
               "Boat_Not_Found_Update" => _messagesService.Boat_Not_Found_Update,
               "Boat_Not_Found_Get" => _messagesService.Boat_Not_Found_Get,
               
               // Boat Image Messages
               "BoatImage_Invalid_Image" => _messagesService.BoatImage_Invalid_Image,
               "BoatImage_Cannot_Decode" => _messagesService.BoatImage_Cannot_Decode,
               "BoatImage_Cannot_Resize" => _messagesService.BoatImage_Cannot_Resize,
               "BoatImage_No_Codec" => _messagesService.BoatImage_No_Codec,
               "BoatImage_At_Least_One" => _messagesService.BoatImage_At_Least_One,
               "BoatImage_Cannot_Be_Empty" => _messagesService.BoatImage_Cannot_Be_Empty,
               "BoatImage_Invalid_Format" => _messagesService.BoatImage_Invalid_Format,
               
               // Authentication Messages
               "Auth_User_Not_Identified" => _messagesService.Auth_User_Not_Identified,
               "Auth_User_Invalid" => _messagesService.Auth_User_Invalid,
               
               // Payment Messages
               "Payment_Create_Error" => _messagesService.Payment_Create_Error,
               "Payment_Create_Asaas_Error" => _messagesService.Payment_Create_Asaas_Error,
               "Payment_Create_Internal_Error" => _messagesService.Payment_Create_Internal_Error,
               "Payment_Token_Error" => _messagesService.Payment_Token_Error,
               "Payment_Card_Create_Error" => _messagesService.Payment_Card_Create_Error,
               "Payment_Card_Asaas_Error" => _messagesService.Payment_Card_Asaas_Error,
               "Payment_Card_Internal_Error" => _messagesService.Payment_Card_Internal_Error,
               "Payment_Get_Error" => _messagesService.Payment_Get_Error,
               "Payment_Get_Asaas_Error" => _messagesService.Payment_Get_Asaas_Error,
               "Payment_Get_Internal_Error" => _messagesService.Payment_Get_Internal_Error,
               "Payment_QRCode_Error" => _messagesService.Payment_QRCode_Error,
               "Payment_QRCode_Asaas_Error" => _messagesService.Payment_QRCode_Asaas_Error,
               "Payment_QRCode_Internal_Error" => _messagesService.Payment_QRCode_Internal_Error,
               "Payment_Boleto_Error" => _messagesService.Payment_Boleto_Error,
               "Payment_Boleto_Asaas_Error" => _messagesService.Payment_Boleto_Asaas_Error,
               "Payment_Boleto_Internal_Error" => _messagesService.Payment_Boleto_Internal_Error,
               "Payment_Refund_Error" => _messagesService.Payment_Refund_Error,
               "Payment_Refund_Asaas_Error" => _messagesService.Payment_Refund_Asaas_Error,
               "Payment_Refund_Internal_Error" => _messagesService.Payment_Refund_Internal_Error,
               "Payment_Cannot_Delete_Paid" => _messagesService.Payment_Cannot_Delete_Paid,
               
               // Scheduled Tour Messages
               "ScheduledTour_Not_Found" => _messagesService.ScheduledTour_Not_Found,
               "ScheduledTour_Conflict" => _messagesService.ScheduledTour_Conflict,
               "ScheduledTour_Cannot_Delete_Completed" => _messagesService.ScheduledTour_Cannot_Delete_Completed,
               "ScheduledTour_Cannot_Edit_Status" => _messagesService.ScheduledTour_Cannot_Edit_Status,
               "ScheduledTour_Start_After_End" => _messagesService.ScheduledTour_Start_After_End,
               "ScheduledTour_Only_Scheduled_Can_Start" => _messagesService.ScheduledTour_Only_Scheduled_Can_Start,
               "ScheduledTour_Only_Started_Can_Complete" => _messagesService.ScheduledTour_Only_Started_Can_Complete,
               "ScheduledTour_Already_Canceled" => _messagesService.ScheduledTour_Already_Canceled,
               "ScheduledTour_Cannot_Cancel_Completed" => _messagesService.ScheduledTour_Cannot_Cancel_Completed,
               "ScheduledTour_Only_Scheduled_Started_Can_Suspend" => _messagesService.ScheduledTour_Only_Scheduled_Started_Can_Suspend,
               "ScheduledTour_Already_Suspended" => _messagesService.ScheduledTour_Already_Suspended,
               "ScheduledTour_Only_Suspended_Can_Reactive" => _messagesService.ScheduledTour_Only_Suspended_Can_Reactive,
               "ScheduledTour_Seats_Negative" => _messagesService.ScheduledTour_Seats_Negative,
               "ScheduledTour_Notes_Too_Long" => _messagesService.ScheduledTour_Notes_Too_Long,
               "ScheduledTour_Boat_Required" => _messagesService.ScheduledTour_Boat_Required,
               "ScheduledTour_Date_Past" => _messagesService.ScheduledTour_Date_Past,
               "ScheduledTour_Seats_Too_High" => _messagesService.ScheduledTour_Seats_Too_High,
               "ScheduledTour_Status_Not_Implemented" => _messagesService.ScheduledTour_Status_Not_Implemented,
               
               // Review Messages
               "Review_Already_Exists" => _messagesService.Review_Already_Exists,
               "Review_Not_Found" => _messagesService.Review_Not_Found,
               "Review_Deleted_Success" => _messagesService.Review_Deleted_Success,
               "Review_Delete_Error" => _messagesService.Review_Delete_Error,
               "Review_Updated_Success" => _messagesService.Review_Updated_Success,
               "Review_Update_Error" => _messagesService.Review_Update_Error,
               
               // Advertisement Messages
               "Advertisement_Already_Approved" => _messagesService.Advertisement_Already_Approved,
               "Advertisement_Cannot_Approve_Canceled" => _messagesService.Advertisement_Cannot_Approve_Canceled,
               "Advertisement_Cannot_Reject_Approved" => _messagesService.Advertisement_Cannot_Reject_Approved,
               "Advertisement_Cannot_Reject_Canceled" => _messagesService.Advertisement_Cannot_Reject_Canceled,
               "Advertisement_Only_Approved_Can_Suspend" => _messagesService.Advertisement_Only_Approved_Can_Suspend,
               "Advertisement_Only_Suspended_Can_Reactive" => _messagesService.Advertisement_Only_Suspended_Can_Reactive,
               "Advertisement_Only_Approved_Can_Pause" => _messagesService.Advertisement_Only_Approved_Can_Pause,
               "Advertisement_Only_Paused_Can_Unpause" => _messagesService.Advertisement_Only_Paused_Can_Unpause,
               "Advertisement_Already_Canceled" => _messagesService.Advertisement_Already_Canceled,
               "Advertisement_Cannot_Cancel_Completed" => _messagesService.Advertisement_Cannot_Cancel_Completed,
               "Advertisement_Only_Approved_Paused_Can_Complete" => _messagesService.Advertisement_Only_Approved_Paused_Can_Complete,
               "Advertisement_ScheduledTour_Required" => _messagesService.Advertisement_ScheduledTour_Required,
               "Advertisement_Customer_Required" => _messagesService.Advertisement_Customer_Required,
               "Advertisement_Seats_Invalid" => _messagesService.Advertisement_Seats_Invalid,
               "Advertisement_Seats_Too_High" => _messagesService.Advertisement_Seats_Too_High,
               "Advertisement_Total_Invalid" => _messagesService.Advertisement_Total_Invalid,
               "Advertisement_Total_Too_High" => _messagesService.Advertisement_Total_Too_High,
               
               // Chat Messages
               "ChatMessage_Not_Found" => _messagesService.ChatMessage_Not_Found,
               "ChatMessage_Deleted_Success" => _messagesService.ChatMessage_Deleted_Success,
               "ChatMessage_Marked_Read" => _messagesService.ChatMessage_Marked_Read,
               "ChatMessage_Marked_Unread" => _messagesService.ChatMessage_Marked_Unread,
               
               // Webhook Messages
               "Webhook_Invalid_Token" => _messagesService.Webhook_Invalid_Token,
               "Webhook_Received" => _messagesService.Webhook_Received,
               "Webhook_Process_Error" => _messagesService.Webhook_Process_Error,
               "Webhook_Internal_Error" => _messagesService.Webhook_Internal_Error,
               "Webhook_Processing" => _messagesService.Webhook_Processing,
               "Webhook_No_Payment_Data" => _messagesService.Webhook_No_Payment_Data,
               "Webhook_Processed_Success" => _messagesService.Webhook_Processed_Success,
               
               // General Error Messages
               "Error_Problem_Request" => _messagesService.Error_Problem_Request,
               "Error_Not_Authorized_Title" => _messagesService.Error_Not_Authorized_Title,
               "Error_Not_Authorized_Message" => _messagesService.Error_Not_Authorized_Message,
               "Error_Forbidden_Title" => _messagesService.Error_Forbidden_Title,
               "Error_Forbidden_Message" => _messagesService.Error_Forbidden_Message,
               "Error_Not_Found_Title" => _messagesService.Error_Not_Found_Title,
               "Error_Not_Found_Message" => _messagesService.Error_Not_Found_Message,
               "Error_Bad_Request_Title" => _messagesService.Error_Bad_Request_Title,
               "Error_Bad_Request_Message" => _messagesService.Error_Bad_Request_Message,
               "Error_Conflict_Title" => _messagesService.Error_Conflict_Title,
               "Error_Conflict_Message" => _messagesService.Error_Conflict_Message,
               "Error_Validation_Title" => _messagesService.Error_Validation_Title,
               "Error_Max_Attempts_Reached" => _messagesService.Error_Max_Attempts_Reached,
               "Error_Record_Not_Found" => _messagesService.Error_Record_Not_Found,
               
               // Configuration Messages
               "Config_S3_Bucket_Required" => _messagesService.Config_S3_Bucket_Required,
               "Config_Hangfire_User_Required" => _messagesService.Config_Hangfire_User_Required,
               "Config_Hangfire_Password_Required" => _messagesService.Config_Hangfire_Password_Required,
               
               // Security Messages
               "Security_Path_Traversal" => _messagesService.Security_Path_Traversal,
               "Security_Invalid_File_Url" => _messagesService.Security_Invalid_File_Url,
               
               // Template Messages
               "Template_Path_Null_Empty" => _messagesService.Template_Path_Null_Empty,
               "Template_Not_Found" => _messagesService.Template_Not_Found,
               "Template_Html_Empty" => _messagesService.Template_Html_Empty,
               
               // Cache Messages
               "Cache_Redis_Not_Configured" => _messagesService.Cache_Redis_Not_Configured,
               
               // IBGE Messages
               "Ibge_No_States_Found" => _messagesService.Ibge_No_States_Found,
               "Ibge_Error_States" => _messagesService.Ibge_Error_States,
               "Ibge_No_Cities_Found" => _messagesService.Ibge_No_Cities_Found,
               "Ibge_Error_Cities" => _messagesService.Ibge_Error_Cities,
               
               // Database Messages
               "Database_Commit_Error" => _messagesService.Database_Commit_Error,
               
               // Email Messages
               "Email_Send_Success" => _messagesService.Email_Send_Success,
               "Email_Send_Failed" => _messagesService.Email_Send_Failed,
               "Email_Send_Error" => _messagesService.Email_Send_Error,
               "Email_SendGrid_Key_Required" => _messagesService.Email_SendGrid_Key_Required,
               
               // Token Messages
               "Token_Generate_Error" => _messagesService.Token_Generate_Error,
               "Token_Refresh_Error" => _messagesService.Token_Refresh_Error,
               
               // Validation Messages (Requests)
               "Validation_Page_Required" => _messagesService.Validation_Page_Required,
               "Validation_PageSize_Required" => _messagesService.Validation_PageSize_Required,
               "Validation_Page_Greater_Zero" => _messagesService.Validation_Page_Greater_Zero,
               "Validation_PageSize_Greater_Zero" => _messagesService.Validation_PageSize_Greater_Zero,
               "Validation_PageSize_Max_100" => _messagesService.Validation_PageSize_Max_100,
               "Validation_Mark_Read_Request_Required" => _messagesService.Validation_Mark_Read_Request_Required,
               "Validation_Invalid_Tour_Status" => _messagesService.Validation_Invalid_Tour_Status,
               "Validation_Reason_Too_Long" => _messagesService.Validation_Reason_Too_Long,
               "Validation_Tour_Date_Past" => _messagesService.Validation_Tour_Date_Past,
               "Validation_Start_Time_Required" => _messagesService.Validation_Start_Time_Required,
               "Validation_End_Time_Required" => _messagesService.Validation_End_Time_Required,
               "Validation_End_Time_After_Start" => _messagesService.Validation_End_Time_After_Start,
               "Validation_Available_Seats_Greater_Zero" => _messagesService.Validation_Available_Seats_Greater_Zero,
               "Validation_Available_Seats_Max_1000" => _messagesService.Validation_Available_Seats_Max_1000,
               "Validation_Notes_Too_Long" => _messagesService.Validation_Notes_Too_Long,
               "Validation_Booking_Id_Required" => _messagesService.Validation_Booking_Id_Required,
               "Validation_Boat_Id_Required" => _messagesService.Validation_Boat_Id_Required,
               "Validation_Customer_Id_Required" => _messagesService.Validation_Customer_Id_Required,
               "Validation_Rating_Between_1_5" => _messagesService.Validation_Rating_Between_1_5,
               "Validation_Comment_Too_Long" => _messagesService.Validation_Comment_Too_Long,
               "Validation_Refund_Value_Greater_Zero" => _messagesService.Validation_Refund_Value_Greater_Zero,
               "Validation_Refund_Reason_Required" => _messagesService.Validation_Refund_Reason_Required,
               "Validation_Refund_Reason_Too_Long" => _messagesService.Validation_Refund_Reason_Too_Long,
               
               // CNPJ/GTIN Validation Messages
               "Validation_Cnpj_Base_Invalid" => _messagesService.Validation_Cnpj_Base_Invalid,
               "Validation_Gtin_Digits_Only" => _messagesService.Validation_Gtin_Digits_Only,
               "Validation_Gtin_Invalid_Length" => _messagesService.Validation_Gtin_Invalid_Length,
               
               // Date Validation Messages
               "Validation_Invalid_Year" => _messagesService.Validation_Invalid_Year,
               "Validation_Invalid_Month" => _messagesService.Validation_Invalid_Month,
               "Validation_Invalid_Day" => _messagesService.Validation_Invalid_Day,
               
               // System Messages
               "System_Unknown_Error" => _messagesService.System_Unknown_Error,
               "System_Expected_Decimal" => _messagesService.System_Expected_Decimal,
               "System_Request_Id_Set" => _messagesService.System_Request_Id_Set,
               "System_Validation_Title" => _messagesService.System_Validation_Title,
               
               // Payment Split Messages
               "PaymentSplit_Either_Fixed_Percentual" => _messagesService.PaymentSplit_Either_Fixed_Percentual,
               _ => messageKey // Retorna a chave original se não encontrar correspondência
         };
    }
}
