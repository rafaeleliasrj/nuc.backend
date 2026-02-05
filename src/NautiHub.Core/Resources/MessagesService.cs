using Microsoft.Extensions.Localization;

namespace NautiHub.Core.Resources;

/// <summary>
/// Serviço centralizado para mensagens localizadas do sistema
/// </summary>
public class MessagesService
{
    private readonly IStringLocalizer _localizer;

    public MessagesService(IStringLocalizerFactory factory)
    {
        // Cria o localizer para o resource "Messages" no assembly atual
        var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        _localizer = factory.Create("Messages", assemblyName ?? "NautiHub.Core");
    }

    // Mensagens de Embarcação
    public string Boat_Already_Exists => _localizer["Boat_Already_Exists"];
    public string Boat_Not_Found => _localizer["Boat_Not_Found"];
    public string Boat_Name_Required => _localizer["Boat_Name_Required"];
    public string Boat_Name_Too_Long => _localizer["Boat_Name_Too_Long"];
    public string Boat_Description_Required => _localizer["Boat_Description_Required"];
    public string Boat_Description_Too_Long => _localizer["Boat_Description_Too_Long"];
    public string Boat_Document_Required => _localizer["Boat_Document_Required"];
    public string Boat_Document_Too_Long => _localizer["Boat_Document_Too_Long"];
    public string Boat_Capacity_Invalid => _localizer["Boat_Capacity_Invalid"];
    public string Boat_Capacity_Too_High => _localizer["Boat_Capacity_Too_High"];
    public string Boat_Price_Invalid => _localizer["Boat_Price_Invalid"];
    public string Boat_Price_Too_High => _localizer["Boat_Price_Too_High"];
    public string Boat_City_Required => _localizer["Boat_City_Required"];
    public string Boat_City_Too_Long => _localizer["Boat_City_Too_Long"];
    public string Boat_State_Required => _localizer["Boat_State_Required"];
    public string Boat_State_Invalid_Format => _localizer["Boat_State_Invalid_Format"];
    public string Boat_Only_Approved_Can_Be_Active => _localizer["Boat_Only_Approved_Can_Be_Active"];
    public string Boat_Already_Approved => _localizer["Boat_Already_Approved"];
    public string Boat_Cannot_Reject_Approved => _localizer["Boat_Cannot_Reject_Approved"];
    public string Boat_Only_Approved_Can_Be_Suspended => _localizer["Boat_Only_Approved_Can_Be_Suspended"];
    public string Boat_Only_Suspended_Can_Be_Reactived => _localizer["Boat_Only_Suspended_Can_Be_Reactived"];
    public string Boat_Not_Found_Update => _localizer["Boat_Not_Found_Update"];
    public string Boat_Not_Found_Get => _localizer["Boat_Not_Found_Get"];
    
    // Mensagens de Imagem
    public string BoatImage_Not_Found => _localizer["BoatImage_Not_Found"];
    public string BoatImage_Invalid_Image => _localizer["BoatImage_Invalid_Image"];
    public string BoatImage_Cannot_Decode => _localizer["BoatImage_Cannot_Decode"];
    public string BoatImage_Cannot_Resize => _localizer["BoatImage_Cannot_Resize"];
    public string BoatImage_No_Codec => _localizer["BoatImage_No_Codec"];
    public string BoatImage_At_Least_One => _localizer["BoatImage_At_Least_One"];
    public string BoatImage_Cannot_Be_Empty => _localizer["BoatImage_Cannot_Be_Empty"];
    public string BoatImage_Invalid_Format => _localizer["BoatImage_Invalid_Format"];
    
    // Mensagens de Autenticação
    public string Auth_User_Created => _localizer["Auth_User_Created"];
    public string Auth_User_Logged_In => _localizer["Auth_User_Logged_In"];
    public string Auth_Account_Locked => _localizer["Auth_Account_Locked"];
    public string Auth_Invalid_Credentials => _localizer["Auth_Invalid_Credentials"];
    public string Auth_Email_Password_Required => _localizer["Auth_Email_Password_Required"];
    public string Auth_Email_In_Use => _localizer["Auth_Email_In_Use"];
    public string Auth_User_Not_Found => _localizer["Auth_User_Not_Found"];
    public string Auth_Account_Not_Allowed => _localizer["Auth_Account_Not_Allowed"];
    public string Auth_Logout_Success => _localizer["Auth_Logout_Success"];
    public string Auth_Logout_Error => _localizer["Auth_Logout_Error"];
    public string Auth_Add_Role_Success => _localizer["Auth_Add_Role_Success"];
    public string Auth_Add_Role_Error => _localizer["Auth_Add_Role_Error"];
    public string Auth_User_Not_Identified => _localizer["Auth_User_Not_Identified"];
    public string Auth_User_Invalid => _localizer["Auth_User_Invalid"];
    
    // Mensagens de Pagamento
    public string Payment_Not_Found => _localizer["Payment_Not_Found"];
    public string Payment_Created_Success => _localizer["Payment_Created_Success"];
    public string Payment_Invalid_Method => _localizer["Payment_Invalid_Method"];
    public string Payment_Insufficient_Value => _localizer["Payment_Insufficient_Value"];
    public string Payment_Card_Expired => _localizer["Payment_Card_Expired"];
    public string Payment_Invalid_Document => _localizer["Payment_Invalid_Document"];
    public string Payment_Split_Invalid_Total => _localizer["Payment_Split_Invalid_Total"];
    public string Payment_Webhook_Invalid_Token => _localizer["Payment_Webhook_Invalid_Token"];
    public string Payment_Refund_Not_Allowed => _localizer["Payment_Refund_Not_Allowed"];
    public string Payment_Overdue_Notice => _localizer["Payment_Overdue_Notice"];
    public string Payment_Chargeback_Received => _localizer["Payment_Chargeback_Received"];
    public string Payment_QRCode_Not_Allowed => _localizer["Payment_QRCode_Not_Allowed"];
    public string Payment_No_Asaas_Id => _localizer["Payment_No_Asaas_Id"];
    public string Payment_Asaas_Error => _localizer["Payment_Asaas_Error"];
    public string Payment_General_Error => _localizer["Payment_General_Error"];
    public string Payment_Booking_Not_Found => _localizer["Payment_Booking_Not_Found"];
    public string Payment_List_Error => _localizer["Payment_List_Error"];
    public string Payment_Get_Status_Error => _localizer["Payment_Get_Status_Error"];
    public string Payment_Create_Error => _localizer["Payment_Create_Error"];
    public string Payment_Create_Asaas_Error => _localizer["Payment_Create_Asaas_Error"];
    public string Payment_Create_Internal_Error => _localizer["Payment_Create_Internal_Error"];
    public string Payment_Token_Error => _localizer["Payment_Token_Error"];
    public string Payment_Card_Create_Error => _localizer["Payment_Card_Create_Error"];
    public string Payment_Card_Asaas_Error => _localizer["Payment_Card_Asaas_Error"];
    public string Payment_Card_Internal_Error => _localizer["Payment_Card_Internal_Error"];
    public string Payment_Get_Error => _localizer["Payment_Get_Error"];
    public string Payment_Get_Asaas_Error => _localizer["Payment_Get_Asaas_Error"];
    public string Payment_Get_Internal_Error => _localizer["Payment_Get_Internal_Error"];
    public string Payment_QRCode_Error => _localizer["Payment_QRCode_Error"];
    public string Payment_QRCode_Asaas_Error => _localizer["Payment_QRCode_Asaas_Error"];
    public string Payment_QRCode_Internal_Error => _localizer["Payment_QRCode_Internal_Error"];
    public string Payment_Boleto_Error => _localizer["Payment_Boleto_Error"];
    public string Payment_Boleto_Asaas_Error => _localizer["Payment_Boleto_Asaas_Error"];
    public string Payment_Boleto_Internal_Error => _localizer["Payment_Boleto_Internal_Error"];
    public string Payment_Refund_Error => _localizer["Payment_Refund_Error"];
    public string Payment_Refund_Asaas_Error => _localizer["Payment_Refund_Asaas_Error"];
    public string Payment_Refund_Internal_Error => _localizer["Payment_Refund_Internal_Error"];
    public string Payment_Cannot_Delete_Paid => _localizer["Payment_Cannot_Delete_Paid"];
    
    // Mensagens de Passeio Agendado (ScheduledTour)
    public string ScheduledTour_Not_Found => _localizer["ScheduledTour_Not_Found"];
    public string ScheduledTour_Conflict => _localizer["ScheduledTour_Conflict"];
    public string ScheduledTour_Cannot_Delete_Completed => _localizer["ScheduledTour_Cannot_Delete_Completed"];
    public string ScheduledTour_Cannot_Edit_Status => _localizer["ScheduledTour_Cannot_Edit_Status"];
    public string ScheduledTour_Start_After_End => _localizer["ScheduledTour_Start_After_End"];
    public string ScheduledTour_Only_Scheduled_Can_Start => _localizer["ScheduledTour_Only_Scheduled_Can_Start"];
    public string ScheduledTour_Only_Started_Can_Complete => _localizer["ScheduledTour_Only_Started_Can_Complete"];
    public string ScheduledTour_Already_Canceled => _localizer["ScheduledTour_Already_Canceled"];
    public string ScheduledTour_Cannot_Cancel_Completed => _localizer["ScheduledTour_Cannot_Cancel_Completed"];
    public string ScheduledTour_Only_Scheduled_Started_Can_Suspend => _localizer["ScheduledTour_Only_Scheduled_Started_Can_Suspend"];
    public string ScheduledTour_Already_Suspended => _localizer["ScheduledTour_Already_Suspended"];
    public string ScheduledTour_Only_Suspended_Can_Reactive => _localizer["ScheduledTour_Only_Suspended_Can_Reactive"];
    public string ScheduledTour_Seats_Negative => _localizer["ScheduledTour_Seats_Negative"];
    public string ScheduledTour_Notes_Too_Long => _localizer["ScheduledTour_Notes_Too_Long"];
    public string ScheduledTour_Boat_Required => _localizer["ScheduledTour_Boat_Required"];
    public string ScheduledTour_Date_Past => _localizer["ScheduledTour_Date_Past"];
    public string ScheduledTour_Seats_Too_High => _localizer["ScheduledTour_Seats_Too_High"];
    public string ScheduledTour_Status_Not_Implemented => _localizer["ScheduledTour_Status_Not_Implemented"];
    
    // Mensagens de Avaliação (Review)
    public string Review_Already_Exists => _localizer["Review_Already_Exists"];
    public string Review_Not_Found => _localizer["Review_Not_Found"];
    public string Review_Deleted_Success => _localizer["Review_Deleted_Success"];
    public string Review_Delete_Error => _localizer["Review_Delete_Error"];
    public string Review_Updated_Success => _localizer["Review_Updated_Success"];
    public string Review_Update_Error => _localizer["Review_Update_Error"];
    
    // Mensagens de Anúncio (Advertisement)
    public string Advertisement_Already_Approved => _localizer["Advertisement_Already_Approved"];
    public string Advertisement_Cannot_Approve_Canceled => _localizer["Advertisement_Cannot_Approve_Canceled"];
    public string Advertisement_Cannot_Reject_Approved => _localizer["Advertisement_Cannot_Reject_Approved"];
    public string Advertisement_Cannot_Reject_Canceled => _localizer["Advertisement_Cannot_Reject_Canceled"];
    public string Advertisement_Only_Approved_Can_Suspend => _localizer["Advertisement_Only_Approved_Can_Suspend"];
    public string Advertisement_Only_Suspended_Can_Reactive => _localizer["Advertisement_Only_Suspended_Can_Reactive"];
    public string Advertisement_Only_Approved_Can_Pause => _localizer["Advertisement_Only_Approved_Can_Pause"];
    public string Advertisement_Only_Paused_Can_Unpause => _localizer["Advertisement_Only_Paused_Can_Unpause"];
    public string Advertisement_Already_Canceled => _localizer["Advertisement_Already_Canceled"];
    public string Advertisement_Cannot_Cancel_Completed => _localizer["Advertisement_Cannot_Cancel_Completed"];
    public string Advertisement_Only_Approved_Paused_Can_Complete => _localizer["Advertisement_Only_Approved_Paused_Can_Complete"];
    public string Advertisement_ScheduledTour_Required => _localizer["Advertisement_ScheduledTour_Required"];
    public string Advertisement_Customer_Required => _localizer["Advertisement_Customer_Required"];
    public string Advertisement_Seats_Invalid => _localizer["Advertisement_Seats_Invalid"];
    public string Advertisement_Seats_Too_High => _localizer["Advertisement_Seats_Too_High"];
    public string Advertisement_Total_Invalid => _localizer["Advertisement_Total_Invalid"];
    public string Advertisement_Total_Too_High => _localizer["Advertisement_Total_Too_High"];
    
    // Mensagens de Chat
    public string ChatMessage_Not_Found => _localizer["ChatMessage_Not_Found"];
    public string ChatMessage_Deleted_Success => _localizer["ChatMessage_Deleted_Success"];
    public string ChatMessage_Marked_Read => _localizer["ChatMessage_Marked_Read"];
    public string ChatMessage_Marked_Unread => _localizer["ChatMessage_Marked_Unread"];
    
    // Mensagens de Webhook
    public string Webhook_Invalid_Token => _localizer["Webhook_Invalid_Token"];
    public string Webhook_Received => _localizer["Webhook_Received"];
    public string Webhook_Process_Error => _localizer["Webhook_Process_Error"];
    public string Webhook_Internal_Error => _localizer["Webhook_Internal_Error"];
    public string Webhook_Processing => _localizer["Webhook_Processing"];
    public string Webhook_No_Payment_Data => _localizer["Webhook_No_Payment_Data"];
    public string Webhook_Processed_Success => _localizer["Webhook_Processed_Success"];
    
    // Mensagens Gerais de Erro
    public string Error_Internal_Server => _localizer["Error_Internal_Server"];
    public string Error_Registering_User => _localizer["Error_Registering_User"];
    public string Error_Login => _localizer["Error_Login"];
    public string Error_Internal_Server_Generic => _localizer["Error_Internal_Server_Generic"];
    public string Error_Problem_Request => _localizer["Error_Problem_Request"];
    public string Error_Not_Authorized_Title => _localizer["Error_Not_Authorized_Title"];
    public string Error_Not_Authorized_Message => _localizer["Error_Not_Authorized_Message"];
    public string Error_Forbidden_Title => _localizer["Error_Forbidden_Title"];
    public string Error_Forbidden_Message => _localizer["Error_Forbidden_Message"];
    public string Error_Not_Found_Title => _localizer["Error_Not_Found_Title"];
    public string Error_Not_Found_Message => _localizer["Error_Not_Found_Message"];
    public string Error_Bad_Request_Title => _localizer["Error_Bad_Request_Title"];
    public string Error_Bad_Request_Message => _localizer["Error_Bad_Request_Message"];
    public string Error_Conflict_Title => _localizer["Error_Conflict_Title"];
    public string Error_Conflict_Message => _localizer["Error_Conflict_Message"];
    public string Error_Validation_Title => _localizer["Error_Validation_Title"];
    public string Error_Max_Attempts_Reached => _localizer["Error_Max_Attempts_Reached"];
    public string Error_Record_Not_Found => _localizer["Error_Record_Not_Found"];
    
    // Mensagens de Configuração
    public string Config_S3_Bucket_Required => _localizer["Config_S3_Bucket_Required"];
    public string Config_Hangfire_User_Required => _localizer["Config_Hangfire_User_Required"];
    public string Config_Hangfire_Password_Required => _localizer["Config_Hangfire_Password_Required"];
    
    // Mensagens de Segurança
    public string Security_Path_Traversal => _localizer["Security_Path_Traversal"];
    public string Security_Invalid_File_Url => _localizer["Security_Invalid_File_Url"];
    
    // Mensagens de Template
    public string Template_Path_Null_Empty => _localizer["Template_Path_Null_Empty"];
    public string Template_Not_Found => _localizer["Template_Not_Found"];
    public string Template_Html_Empty => _localizer["Template_Html_Empty"];
    
    // Mensagens de Cache
    public string Cache_Redis_Not_Configured => _localizer["Cache_Redis_Not_Configured"];
    
    // Mensagens de IBGE
    public string Ibge_No_States_Found => _localizer["Ibge_No_States_Found"];
    public string Ibge_Error_States => _localizer["Ibge_Error_States"];
    public string Ibge_No_Cities_Found => _localizer["Ibge_No_Cities_Found"];
    public string Ibge_Error_Cities => _localizer["Ibge_Error_Cities"];
    
    // Mensagens de Database
    public string Database_Commit_Error => _localizer["Database_Commit_Error"];
    
    // Mensagens de Email
    public string Email_Send_Success => _localizer["Email_Send_Success"];
    public string Email_Send_Failed => _localizer["Email_Send_Failed"];
    public string Email_Send_Error => _localizer["Email_Send_Error"];
    public string Email_SendGrid_Key_Required => _localizer["Email_SendGrid_Key_Required"];
    
    // Mensagens de Token
    public string Token_Generate_Error => _localizer["Token_Generate_Error"];
    public string Token_Refresh_Error => _localizer["Token_Refresh_Error"];
    
    // Mensagens de Validação (Clientes/Request)
    public string Validation_Page_Required => _localizer["Validation_Page_Required"];
    public string Validation_PageSize_Required => _localizer["Validation_PageSize_Required"];
    public string Validation_Page_Greater_Zero => _localizer["Validation_Page_Greater_Zero"];
    public string Validation_PageSize_Greater_Zero => _localizer["Validation_PageSize_Greater_Zero"];
    public string Validation_PageSize_Max_100 => _localizer["Validation_PageSize_Max_100"];
    public string Validation_Mark_Read_Request_Required => _localizer["Validation_Mark_Read_Request_Required"];
    public string Validation_Invalid_Tour_Status => _localizer["Validation_Invalid_Tour_Status"];
    public string Validation_Reason_Too_Long => _localizer["Validation_Reason_Too_Long"];
    public string Validation_Tour_Date_Past => _localizer["Validation_Tour_Date_Past"];
    public string Validation_Start_Time_Required => _localizer["Validation_Start_Time_Required"];
    public string Validation_End_Time_Required => _localizer["Validation_End_Time_Required"];
    public string Validation_End_Time_After_Start => _localizer["Validation_End_Time_After_Start"];
    public string Validation_Available_Seats_Greater_Zero => _localizer["Validation_Available_Seats_Greater_Zero"];
    public string Validation_Available_Seats_Max_1000 => _localizer["Validation_Available_Seats_Max_1000"];
    public string Validation_Notes_Too_Long => _localizer["Validation_Notes_Too_Long"];
    public string Validation_Booking_Id_Required => _localizer["Validation_Booking_Id_Required"];
    public string Validation_Boat_Id_Required => _localizer["Validation_Boat_Id_Required"];
    public string Validation_Customer_Id_Required => _localizer["Validation_Customer_Id_Required"];
    public string Validation_Rating_Between_1_5 => _localizer["Validation_Rating_Between_1_5"];
    public string Validation_Comment_Too_Long => _localizer["Validation_Comment_Too_Long"];
    public string Validation_Refund_Value_Greater_Zero => _localizer["Validation_Refund_Value_Greater_Zero"];
    public string Validation_Refund_Reason_Required => _localizer["Validation_Refund_Reason_Required"];
    public string Validation_Refund_Reason_Too_Long => _localizer["Validation_Refund_Reason_Too_Long"];
    
    // Mensagens de Validação - Pagamento (em Inglês - para compatibilidade)
    public string Validation_Payment_Booking_Id_Required => _localizer["Validation_Payment_Booking_Id_Required"];
    public string Validation_Payment_Customer_Id_Required => _localizer["Validation_Payment_Customer_Id_Required"];
    public string Validation_Payment_Customer_Id_Too_Long => _localizer["Validation_Payment_Customer_Id_Too_Long"];
    public string Validation_Payment_Value_Greater_Zero => _localizer["Validation_Payment_Value_Greater_Zero"];
    public string Validation_Payment_Value_Max_100k => _localizer["Validation_Payment_Value_Max_100k"];
    public string Validation_Payment_Method_Invalid => _localizer["Validation_Payment_Method_Invalid"];
    public string Validation_Payment_Description_Required => _localizer["Validation_Payment_Description_Required"];
    public string Validation_Payment_Description_Too_Long => _localizer["Validation_Payment_Description_Too_Long"];
    public string Validation_Payment_Due_Date_Future => _localizer["Validation_Payment_Due_Date_Future"];
    public string Validation_Payment_External_Ref_Too_Long => _localizer["Validation_Payment_External_Ref_Too_Long"];
    public string Validation_Payment_Card_Info_Required => _localizer["Validation_Payment_Card_Info_Required"];
    public string Validation_Payment_Card_Holder_Required => _localizer["Validation_Payment_Card_Holder_Required"];
    public string Validation_Payment_Card_Holder_Too_Long => _localizer["Validation_Payment_Card_Holder_Too_Long"];
    public string Validation_Payment_Card_Number_Required => _localizer["Validation_Payment_Card_Number_Required"];
    public string Validation_Payment_Card_Number_Invalid => _localizer["Validation_Payment_Card_Number_Invalid"];
    public string Validation_Payment_Expiry_Month_Invalid => _localizer["Validation_Payment_Expiry_Month_Invalid"];
    public string Validation_Payment_Expiry_Year_Past => _localizer["Validation_Payment_Expiry_Year_Past"];
    public string Validation_Payment_Expiry_Year_Future => _localizer["Validation_Payment_Expiry_Year_Future"];
    public string Validation_Payment_Cvv_Required => _localizer["Validation_Payment_Cvv_Required"];
    public string Validation_Payment_Cvv_Invalid_Digits => _localizer["Validation_Payment_Cvv_Invalid_Digits"];
    public string Validation_Payment_Cvv_Numbers_Only => _localizer["Validation_Payment_Cvv_Numbers_Only"];
    public string Validation_Payment_Document_Required => _localizer["Validation_Payment_Document_Required"];
    public string Validation_Payment_Document_Invalid => _localizer["Validation_Payment_Document_Invalid"];
    public string Validation_Payment_Email_Required => _localizer["Validation_Payment_Email_Required"];
    public string Validation_Payment_Email_Invalid => _localizer["Validation_Payment_Email_Invalid"];
    public string Validation_Payment_Postal_Code_Required => _localizer["Validation_Payment_Postal_Code_Required"];
    public string Validation_Payment_Postal_Code_Too_Long => _localizer["Validation_Payment_Postal_Code_Too_Long"];
    public string Validation_Payment_Address_Required => _localizer["Validation_Payment_Address_Required"];
    public string Validation_Payment_Address_Too_Long => _localizer["Validation_Payment_Address_Too_Long"];
    public string Validation_Payment_Address_Number_Required => _localizer["Validation_Payment_Address_Number_Required"];
    public string Validation_Payment_Address_Number_Too_Long => _localizer["Validation_Payment_Address_Number_Too_Long"];
    public string Validation_Payment_Province_Required => _localizer["Validation_Payment_Province_Required"];
    public string Validation_Payment_Province_Too_Long => _localizer["Validation_Payment_Province_Too_Long"];
    public string Validation_Payment_City_Required => _localizer["Validation_Payment_City_Required"];
    public string Validation_Payment_City_Too_Long => _localizer["Validation_Payment_City_Too_Long"];
    public string Validation_Payment_State_Required => _localizer["Validation_Payment_State_Required"];
    public string Validation_Payment_State_Too_Long => _localizer["Validation_Payment_State_Too_Long"];
    public string Validation_Payment_Country_Required => _localizer["Validation_Payment_Country_Required"];
    public string Validation_Payment_Country_Too_Long => _localizer["Validation_Payment_Country_Too_Long"];
    public string Validation_Payment_Mobile_Required => _localizer["Validation_Payment_Mobile_Required"];
    public string Validation_Payment_Mobile_Too_Long => _localizer["Validation_Payment_Mobile_Too_Long"];
    public string Validation_Payment_Remote_Ip_Required => _localizer["Validation_Payment_Remote_Ip_Required"];
    public string Validation_Payment_Remote_Ip_Invalid => _localizer["Validation_Payment_Remote_Ip_Invalid"];
    public string Validation_Payment_Splits_Either_Fixed_Percentual => _localizer["Validation_Payment_Splits_Either_Fixed_Percentual"];
    public string Validation_Payment_Split_Wallet_Required => _localizer["Validation_Payment_Split_Wallet_Required"];
    public string Validation_Payment_Split_Wallet_Too_Long => _localizer["Validation_Payment_Split_Wallet_Too_Long"];
    public string Validation_Payment_Split_Fixed_Greater_Zero => _localizer["Validation_Payment_Split_Fixed_Greater_Zero"];
    public string Validation_Payment_Split_Percentual_Greater_Zero => _localizer["Validation_Payment_Split_Percentual_Greater_Zero"];
    public string Validation_Payment_Split_Percentual_Max_100 => _localizer["Validation_Payment_Split_Percentual_Max_100"];
    public string Validation_Payment_Split_Description_Too_Long => _localizer["Validation_Payment_Split_Description_Too_Long"];
    
    // Mensagens de Validação - CNPJ/GTIN
    public string Validation_Cnpj_Base_Invalid => _localizer["Validation_Cnpj_Base_Invalid"];
    public string Validation_Gtin_Digits_Only => _localizer["Validation_Gtin_Digits_Only"];
    public string Validation_Gtin_Invalid_Length => _localizer["Validation_Gtin_Invalid_Length"];
    
    // Mensagens de Validação - Data
    public string Validation_Invalid_Year => _localizer["Validation_Invalid_Year"];
    public string Validation_Invalid_Month => _localizer["Validation_Invalid_Month"];
    public string Validation_Invalid_Day => _localizer["Validation_Invalid_Day"];
    
    // Mensagens de Sistema
    public string System_Unknown_Error => _localizer["System_Unknown_Error"];
    public string System_Expected_Decimal => _localizer["System_Expected_Decimal"];
    public string System_Validation_Title => _localizer["System_Validation_Title"];
    public string System_Request_Id_Set => _localizer["System_Request_Id_Set"];
    
    // Mensagens de Split de Pagamento
    public string PaymentSplit_Either_Fixed_Percentual => _localizer["PaymentSplit_Either_Fixed_Percentual"];
}