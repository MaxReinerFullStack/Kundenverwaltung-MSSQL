namespace ViewModel
{
    using System.Data;
    using System.Windows.Input;

    using DataModel;

    using ViewModel.Commands;
    using ViewModel.Notifications;

    public class UserMainViewModel
    {
        private readonly DatabaseContext databaseContext;

        public UserMainViewModel()
        {
            this.SaveCommand = new SaveCommand();
            this.databaseContext = DatabaseContext.Instance;

            this.databaseContext.DataSetHandler.Customers.RowChanged += this.CustomerOnRowChanged;
            this.databaseContext.DataSetHandler.Customers.RowDeleted += this.CustomerOnRoleRowDeleted;
        }

        public DataView Users
        {
            get
            {
                return this.databaseContext.DataSetHandler.User.DefaultView;
            }
        }

        public DataView Roles
        {
            get
            {
                return this.databaseContext.DataSetHandler.Role.DefaultView;
            }
        }

        public ICommand SaveCommand { get; private set; }

        public void OnLoaded()
        {
            this.GenerateDefaultColumns();
            this.InitializeRolesColumns();
        }

        private void GenerateDefaultColumns()
        {
            this.AddTextColumn("First Name", "FirstName");
            this.AddTextColumn("Last Name", "LastName");
        }

        private void InitializeRolesColumns()
        {
            foreach (var role in this.databaseContext.DataSetHandler.Role)
            {
                this.AddRoleColumn(role);
            }
        }

        private void RoleOnRowChanged(object sender, UserRoleDataSet.RoleRowChangeEvent roleRowChangeEvent)
        {
            switch (roleRowChangeEvent.Action)
            {
                case DataRowAction.Change:
                    this.ChangeRoleColumn(roleRowChangeEvent.Row);
                    break;
                case DataRowAction.Add:
                    this.AddRoleColumn(roleRowChangeEvent.Row);
                    break;
            }
        }

        private void RoleOnRoleRowDeleted(object sender, UserRoleDataSet.RoleRowChangeEvent roleRowChangeEvent)
        {
            if (roleRowChangeEvent.Action == DataRowAction.Delete)
            {
                this.DeleteRoleColumn(roleRowChangeEvent.Row);
            }
        }

        private void AddTextColumn(string header, string binding)
        {
            var addTextColumnNotification = new AddTextColumnNotification
            {
                Header = header,
                Binding = binding
            };
            DataColumnService.Instance.AddTextColumn.Raise(addTextColumnNotification);
        }

        private void AddRoleColumn(UserRoleDataSet.RoleRow role)
        {
            var notification = new AddDynamicColumnNotification { Role = role };
            DataColumnService.Instance.AddDynamicColumn.Raise(notification);
        }

        private void ChangeRoleColumn(UserRoleDataSet.RoleRow role)
        {
            var notification = new ChangeDynamicColumnNotification { Role = role };
            DataColumnService.Instance.ChangeDynamicColumn.Raise(notification);
        }

        private void DeleteRoleColumn(UserRoleDataSet.RoleRow role)
        {
            var notification = new DeleteDynamicColumnNotification { Role = role };
            DataColumnService.Instance.DeleteDynamicColumn.Raise(notification);
        }
    }
}