using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HotelManagement
{
    public partial class Services : Form
    {
        public List<string> SelectedServices = new List<string>();
        public bool UpdateMode;
        public int CheckBoxWidth = 0, NumOfServices = 0;
        private HotelDataEF.HotelManagementDatabaseEntities ctx;

        public Services(List<string> text, bool IsUpdate)
        {
            InitializeComponent();
            SelectedServices = text;
            UpdateMode = IsUpdate;
        }

        public void AddService(string Service, double Price)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Services.Load();
            HotelDataEF.Services NewService = new HotelDataEF.Services();
            NewService.Service = Service;
            NewService.Price = Price;
            ctx.Services.Add(NewService);
            ctx.SaveChanges();
        }

        public void DeleteService(int id)
        {
            List<HotelDataEF.Services> ServicesList = (List<HotelDataEF.Services>)(SelectService(id));
            HotelDataEF.Services DesiredService = ServicesList[0];
            ctx.Services.Remove(DesiredService);
            ctx.SaveChanges();
        }

        public object SelectService(int id)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Services.Load();
            var cl = (from s in ctx.Services where s.Id==id select s).ToList();
            return cl;
        }

        public void UpdateService(int old_id, int new_id, string Service, double Price)
        {
            List<HotelDataEF.Services> ServicesList = (List<HotelDataEF.Services>)(SelectService(old_id));
            HotelDataEF.Services DesiredService = ServicesList[0];
            DesiredService.Id = new_id;
            DesiredService.Service = Service;
            DesiredService.Price = Price;
            ctx.SaveChanges();
        }

        public float ServiceCost(int ServiceId)
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Services.Load();
            float price = (float)(from s in ctx.Services where s.Id == ServiceId select s.Price).First();
            return price;
        }

        public object ReadServices()
        {
            ctx = new HotelDataEF.HotelManagementDatabaseEntities();
            ctx.Services.Load();
            var result = (from s in ctx.Services select s).ToList();
            return result;
        }

        public void LoadItems()
        {
            List<HotelDataEF.Services> ServicesList = (List<HotelDataEF.Services>)ReadServices();
            NumOfServices = ServicesList.Count;
            foreach (var Service in ServicesList)
            {
                string item = Service.Service.ToString() + " - " + Service.Price.ToString();
                string index = Service.Id.ToString();
                ServicesIdListBox.Items.Add(index);
                ServicesCheckedListBox.Items.Add(item);
                ServiceLenLabel.Text = item;
                if (CheckBoxWidth < ServiceLenLabel.Size.Width)
                    CheckBoxWidth = ServiceLenLabel.Size.Width;
            }
            CheckBoxWidth += 20;
        }

        private void ChangeSize()
        {
            int CheckBoxHeight = 15 * ServicesCheckedListBox.Items.Count + 4;
            ServicesCheckedListBox.Size = new Size(CheckBoxWidth, CheckBoxHeight);
            int NewFormWidth = 40 + ServicesCheckedListBox.Size.Width;
            int NewFormHeight = 100 + 15 * ServicesCheckedListBox.Items.Count;
            this.Size = new Size(NewFormWidth, NewFormHeight);
            AcceptServicesButton.Location = new Point((this.ClientSize.Width - AcceptServicesButton.Size.Width) / 2,
                CheckBoxHeight + 30);
        }

        private void LoadCheckBoxes()
        {
            string[] list1 = new string[NumOfServices];
            for (int i = 0; i < ServicesIdListBox.Items.Count; i++)
            {
                for(int j=0; j<SelectedServices.Count; j++)
                {
                    if (ServicesIdListBox.Items[i].ToString() == SelectedServices[j].ToString())
                    {
                        list1[i] = SelectedServices[j].ToString();
                        break;
                    }
                    else list1[i] = "0";
                }
            }
                
            for (int MyIndex=0; MyIndex<ServicesCheckedListBox.Items.Count; MyIndex++ )
                if (Convert.ToInt16(list1[MyIndex]) > 0)
                    ServicesCheckedListBox.SetItemCheckState(ServicesCheckedListBox.Items.
                        IndexOf(ServicesCheckedListBox.Items[MyIndex]), CheckState.Checked);
                else ServicesCheckedListBox.SetItemCheckState(ServicesCheckedListBox.Items.
                    IndexOf(ServicesCheckedListBox.Items[MyIndex]), CheckState.Unchecked);
        }

        private void TransferListBoxItems(ListBox from, ListBox to)
        {
            to.Items.Clear();
            foreach (var item in from.Items)
            {
                to.Items.Add(item);
            }
        }

        private void SaveServices()
        {
            UpdateResultLabel();
            HotelManagement hm = this.Owner as HotelManagement;
            if (UpdateMode)
                TransferListBoxItems(ResultListBox, hm.SelectedServicesListBox2);
            else TransferListBoxItems(ResultListBox, hm.SelectedServicesListBox1);
            Dispose();
        }

        private void UpdateResultLabel()
        {
            string[] list1 = new string[NumOfServices];
            int MyIndex = 0;
            foreach (object itemChecked in ServicesCheckedListBox.Items)
            {
                if (ServicesCheckedListBox.GetItemCheckState(ServicesCheckedListBox.Items.IndexOf(itemChecked))
                    == CheckState.Checked)
                {
                    list1[MyIndex] = ServicesIdListBox.Items[MyIndex].ToString();
                }
                else list1[MyIndex] = "0";
                MyIndex++;
            }
            ResultListBox.Items.Clear();
            for (int i = 0; i < list1.Length; i++)
            {
                if(Convert.ToInt16(list1[i])>0)
                {
                    ResultListBox.Items.Add(list1[i]);
                }
            }
        }

        private void AcceptServicesButton_Click(object sender, EventArgs e)
        {
            SaveServices();
        }

        private void Services_Load(object sender, EventArgs e)
        {
            LoadItems();
            LoadCheckBoxes();
            ChangeSize();
        }
    }
}
