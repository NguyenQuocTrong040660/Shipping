namespace UserManagement.Domain.Enums
{
    public struct Roles
    {
        public const string SystemAdministrator = nameof(SystemAdministrator);
        public const string ITAdministrator = nameof(ITAdministrator);
        public const string ShippingDept = nameof(ShippingDept);

        public const string WarehouseSupervisor = nameof(WarehouseSupervisor);
        public const string QASupervisor = nameof(QASupervisor);

        public const string LogisticsDept = nameof(LogisticsDept);
        public const string PlanningDept = nameof(PlanningDept);
        public const string FAQDept = nameof(FAQDept);
    }
}
