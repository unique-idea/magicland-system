﻿using MagicLand_System.Enums;
using MagicLand_System.PayLoad.Request;
using MagicLand_System.PayLoad.Request.Cart;
using MagicLand_System.PayLoad.Request.Checkout;
using MagicLand_System.PayLoad.Request.Student;
using MagicLand_System.PayLoad.Response.Bills;
using MagicLand_System.PayLoad.Response.Students;
using MagicLand_System.PayLoad.Response.WalletTransactions;

namespace MagicLand_System.Services.Interfaces
{
    public interface IWalletTransactionService
    {
        Task<List<WalletTransactionResponse>> GetWalletTransactions(string phone = null, DateTime? startDate = null, DateTime? endDate = null, string? transactionCode = null);
        Task<WalletTransactionResponse> GetWalletTransaction(string id);
        Task<BillTopUpResponse?> GenerateBillTopUpTransactionAsync(Guid id);
        Task<BillPaymentResponse?> GenerateBillPaymentTransactionAssync(string txnRefCode);
        Task<(Guid, string)> GenerateTopUpTransAsync(double money);
        Task<(string, double)> GeneratePaymentTransAsync(List<ItemGenerate> items);
        Task<bool> ValidRegisterAsync(List<StudentScheduleResponse> allStudentSchedules, Guid classId, List<Guid> studentIds);
        //Task<bool> ValidRegisterAsync(List<StudentScheduleResponse>? allStudentSchedules, List<Guid>? studentIds, List<CreateStudentRequest>? studentIfors, Guid classId);
        Task<BillPaymentResponse> CheckoutAsync(List<CheckoutRequest> requests);
        Task<(string, bool)> HandelSuccessReturnDataVnpayAsync(string transactionCode, string signature, string bankCode, TransactionTypeEnum type);
        Task<(string, bool)> HandelFailedReturnDataVnpayAsync(string transactionCode, string signature, string bankCode, TransactionTypeEnum type);
        Task<List<RevenueResponse>> GetRevenueTransactionByTimeAsync(RevenueTimeEnum time);
        Task<BillPaymentResponse> CheckoutByStaff(StaffCheckoutRequest request);
    }
}
