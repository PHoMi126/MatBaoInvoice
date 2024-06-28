using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using SAPbouiCOM;
using MatBaoInvoice.Invoice;

namespace MatBaoInvoice.Event
{
    class B1Events
    {
        private Application SBO_Application;
        private SAPbobsCOM.Company oCompany;
        private Form oFormPopUp;
        private bool bPopUp; //Flag to indicate if the modal form is open.

        public B1Events(Application SBO_Application, SAPbobsCOM.Company oCompany)
        {
            this.SBO_Application = SBO_Application;
            this.oCompany = oCompany;

            SBO_Application.MenuEvent += new _IApplicationEvents_MenuEventEventHandler(SBO_Application_MenuEvent);
            SBO_Application.ItemEvent += new _IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
            SBO_Application.ProgressBarEvent += new _IApplicationEvents_ProgressBarEventEventHandler(SBO_Application_ProgressBarEvent);
            SBO_Application.StatusBarEvent += new _IApplicationEvents_StatusBarEventEventHandler(SBO_Application_StatusBarEvent);
            SBO_Application.FormDataEvent += new _IApplicationEvents_FormDataEventEventHandler(SBO_Application_FormDataEvent);
            SBO_Application.RightClickEvent += new _IApplicationEvents_RightClickEventEventHandler(SBO_Application_RightClickEvent);
        }

        public void SBO_Application_MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            MBInvoice b = new MBInvoice(SBO_Application, oCompany);
            b.MenuEvent(pVal, out BubbleEvent);
        }

        private void SBO_Application_ItemEvent(string formUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            switch (pVal.EventType)
            {
                case BoEventTypes.et_FORM_LOAD:
                    FORM_LOAD(formUID, pVal, BubbleEvent);
                    break;
                case BoEventTypes.et_ITEM_PRESSED:
                    ITEM_PRESSED(formUID, pVal, BubbleEvent);
                    break;
                case BoEventTypes.et_VALIDATE:
                    VALIDATE(formUID, pVal, BubbleEvent);
                    break;
                case BoEventTypes.et_FORM_CLOSE:
                    if ((formUID == "dDate" || formUID == "dUser") & bPopUp)
                        bPopUp = false;
                    else
                        FORM_CLOSE(formUID, pVal, BubbleEvent);
                    break;
                case BoEventTypes.et_CHOOSE_FROM_LIST:
                    CHOOSE_FROM_LIST(formUID, pVal, BubbleEvent);
                    break;
                case BoEventTypes.et_PICKER_CLICKED:
                    PICKER_CLICKED(formUID, pVal, BubbleEvent);
                    break;
                case BoEventTypes.et_GOT_FOCUS:
                    GOT_FOCUS(formUID, pVal, BubbleEvent);
                    break;
                case BoEventTypes.et_FORM_RESIZE:
                    FORM_RESIZE(formUID, pVal, BubbleEvent);
                    break;
                case BoEventTypes.et_COMBO_SELECT:
                    COMBO_SELECT(formUID, pVal, BubbleEvent);
                    break;
                case BoEventTypes.et_CLICK:
                    CLICK(formUID, pVal, out BubbleEvent);
                    break;
                case BoEventTypes.et_KEY_DOWN:
                    KEY_DOWN(formUID, pVal, out BubbleEvent);
                    break;
                case BoEventTypes.et_DOUBLE_CLICK:
                    DOUBLE_CLICK(formUID, pVal, BubbleEvent);
                    break;
                case BoEventTypes.et_MATRIX_LOAD:
                    //MATRIX_LOAD(FormUID, pVal, BubbleEvent);
                    break;
                case BoEventTypes.et_MATRIX_LINK_PRESSED:
                    MATRIX_LINK_PRESSED(formUID, pVal, BubbleEvent);
                    break;
                case BoEventTypes.et_LOST_FOCUS:
                    LOST_FOCUS(formUID, pVal, BubbleEvent);
                    break;
                case BoEventTypes.et_FORM_ACTIVATE:
                    FORM_ACTIVATE(formUID, pVal, BubbleEvent);
                    break;
                case BoEventTypes.et_FORM_DEACTIVATE:
                    if (bPopUp & (formUID == "dDate" || formUID == "dUser"))
                    {
                        oFormPopUp.Select(); //Select the modal form
                        BubbleEvent = false;
                    }
                    else
                        FORM_DEACTIVATE(formUID, pVal, BubbleEvent);
                    break;
                case BoEventTypes.et_DATASOURCE_LOAD:
                    DATASOURCE_LOAD(formUID, pVal, BubbleEvent);
                    break;
                default:
                    break;
            }
        }

        private void SBO_Application_FormDataEvent(ref BusinessObjectInfo businessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            switch (businessObjectInfo.EventType)
            {
                case BoEventTypes.et_FORM_DATA_ADD:
                    FORM_DATA_ADD(businessObjectInfo.FormUID, businessObjectInfo, out BubbleEvent);
                    break;
                case BoEventTypes.et_FORM_DATA_LOAD:
                    FORM_DATA_LOAD(businessObjectInfo.FormUID, businessObjectInfo, BubbleEvent);
                    break;
                case BoEventTypes.et_FORM_DATA_UPDATE:
                    FORM_DATA_UPDATE(businessObjectInfo.FormUID, businessObjectInfo, out BubbleEvent);
                    break;
            }
        }

        private void SBO_Application_RightClickEvent(ref ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        private void SBO_Application_ProgressBarEvent(ref ProgressBarEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }
        private void SBO_Application_StatusBarEvent(string Text, BoStatusBarMessageType MessageType)
        {

        }

        private void FORM_LOAD(string formUID, ItemEvent pVal, bool BubbleEvent)
        {
            BubbleEvent = true;
            MBInvoice v = new MBInvoice(SBO_Application, oCompany);
            v.FORM_LOAD(formUID, pVal, BubbleEvent);
        }

        private void ITEM_PRESSED(string formUID, ItemEvent pVal, bool BubbleEvent)
        {
            BubbleEvent = true;
            MBInvoice v = new MBInvoice(SBO_Application, oCompany);
            v.ITEM_PRESSED(formUID, pVal, BubbleEvent);
        }

        private void VALIDATE(string FormUID, ItemEvent pVal, bool BubbleEvent)
        {

        }

        private void CHOOSE_FROM_LIST(string FormUID, ItemEvent pVal, bool BubbleEvent)
        {

        }

        private void PICKER_CLICKED(string FormUID, ItemEvent pVal, bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        private void GOT_FOCUS(string FormUID, ItemEvent pVal, bool BubbleEvent)
        {

        }

        private void COMBO_SELECT(string FormUID, ItemEvent pVal, bool BubbleEvent)
        {

        }

        private void CLICK(string FormUID, ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        private void KEY_DOWN(string FormUID, ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        private void DOUBLE_CLICK(string FormUID, ItemEvent pVal, bool BubbleEvent)
        {

        }

        private void FORM_CLOSE(string FormUID, ItemEvent pVal, bool BubbleEvent)
        {

        }

        private void FORM_DATA_ADD(string FormUID, BusinessObjectInfo events, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        private void FORM_DATA_LOAD(string FormUID, BusinessObjectInfo events, bool BubbleEvent)
        {
            BubbleEvent = true;
            MBInvoice v = new MBInvoice(SBO_Application, oCompany);
            v.FORM_DATA_LOAD(FormUID, events, out BubbleEvent);
        }

        private void FORM_DATA_UPDATE(string FormUID, BusinessObjectInfo events, out bool BubbleEvent)
        {
            BubbleEvent = true;
            MBInvoice v = new MBInvoice(SBO_Application, oCompany);
            v.FORM_DATA_UPDATE(FormUID, events, out BubbleEvent);
        }

        private void MATRIX_LOAD(string FormUID, ItemEvent pVal, bool BubbleEvent)
        {

        }

        private void MATRIX_LINK_PRESSED(string FormUID, ItemEvent pVal, bool BubbleEvent)
        {

        }

        private void LOST_FOCUS(string FormUID, ItemEvent pVal, bool BubbleEvent)
        {

        }

        private void FORM_RESIZE(string FormUID, ItemEvent pVal, bool BubbleEvent)
        {

        }
        private void FORM_ACTIVATE(string FormUID, ItemEvent pVal, bool BubbleEvent)
        {

        }

        private void FORM_DEACTIVATE(string FormUID, ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        private void DATASOURCE_LOAD(string FormUID, ItemEvent pVal, bool BubbleEvent)
        {

        }

        private void FORM_DEACTIVATE(string FormUID, ItemEvent pVal, bool BubbleEvent)
        {

        }
    }
}
