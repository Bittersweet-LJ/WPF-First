using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using testTreeView.Models;

namespace testTreeView.Services
{
   public  class PropertyDefineService
    {

        public PropertyDefineService()
        {
            
        }

         static PropertyDefineService()
        {
            InitPartPropertyDict();
        }

        private const int CodeUseNameErr = 1;
        private static readonly Dictionary<string, PropertyList> _partToPropertyDict = new Dictionary<string, PropertyList>();

        public static Dictionary<string, PropertyList> GetPartPropertyDict()
        {
            return _partToPropertyDict;
        }
        /// <summary>
        /// 新建零件加入属性字典
        /// </summary>
        /// <param name="partType"></param>
        public static void AddPartToPropertyDict(PartType partType)
        {
            _partToPropertyDict.Add(partType.Code, new PropertyList());
        }
        /// <summary>
        /// 从属性字典中删除零件
        /// </summary>
        /// <param name="partType"></param>
        public static void DelPartFromPropertyDict(PartType partType)
        {
            _partToPropertyDict.Remove(partType.Code);

        }


        /// <summary>
        /// 根据零件获取其属性列表
        /// </summary>
        /// <param name="partType"></param>
        /// <returns></returns>
        internal PropertyList GetPropertyListFromDict(PartType partType)
        {
           if( _partToPropertyDict.TryGetValue(partType.Code, out PropertyList propertyList))
            {
                return propertyList;
            }
            return null;
        }

        /// <summary>
        /// 根据零件获取其所有父级属性列表
        /// </summary>
        /// <param name="partType"></param>
        /// <returns></returns>
        internal PropertyList GetParentPropertyListFromDict(PartType partType)
        {
            ObservableCollection<Property> mergedCollection = new ObservableCollection<Property>();
            GetAllParentPropertyCollection(partType, mergedCollection);

            return new PropertyList(mergedCollection);
        }
        private void GetAllParentPropertyCollection(PartType partType, ObservableCollection<Property> mergedCollection)
        {
            
            if (partType.Parent != null)
            {
                MergeTwoCollection(mergedCollection, GetPropertyListFromDict(partType.Parent).PropertyCollection);
                GetAllParentPropertyCollection(partType.Parent, mergedCollection);
            }
            
        }
        private void MergeTwoCollection(ObservableCollection<Property> TargetCollection, ObservableCollection<Property> SourceCollection)
        {
            foreach(Property item in SourceCollection)
            {
                TargetCollection.Add(item);
            }
        }

        /// <summary>
        /// 检查新建属性名称是否合法
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="errCode"></param>
        /// <returns></returns>
        internal bool NewPropertyNameCanUse(string propertyName, out int errCode)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                errCode = CodeUseNameErr;
                return false;
            }
            else
            {
                errCode = 0;
                return true;
            }
        }

        /// <summary>
        /// 对选定零件创建属性
        /// </summary>
        /// <param name="partType"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        internal Property CreateProperty(PartType partType, string propertyName)
        {
            Property property = new Property(propertyName);
            _partToPropertyDict[partType.Code].PropertyCollection.Add(property);
            return property;
        }

        /// <summary>
        /// 删除指定零件选择的属性
        /// </summary>
        /// <param name="partType"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        internal bool DeleteProperty(PartType partType,Property property)
        {
            return _partToPropertyDict[partType.Code].PropertyCollection.Remove(property);
        }

        #region 初始化属性字典数据
        private static void InitPartPropertyDict()
        {

            //string rootDirectory = "C:/Users/AideSoftware/source/repos/testTreeView/Resources/PropertyDefine.xml";
            string rootDirectory = ConstData.PropertyRootDirectory;
                XDocument xDocument = XDocument.Load(rootDirectory);
                XElement rootElement = xDocument.Element("Root");
                LoadPropertyDictFromXml(rootElement);
            

        }
        private static void LoadPropertyDictFromXml(XElement rootElement)
        {
            foreach(XElement element in rootElement.Elements("Part"))
            {
               
                ObservableCollection<Property> propertyCollection = new ObservableCollection<Property>();
                foreach(XElement propertyElement in element.Elements("Property"))
                {
                    Property property= CreatePropertyFromXml(propertyElement);
                    propertyCollection.Add(property);
                }
                _partToPropertyDict.Add(element.Attribute("code").Value, new PropertyList(propertyCollection));
                
            }
        }

        private static Property CreatePropertyFromXml(XElement xElement)
        {
            Property property = new Property(xElement.Element("Name").Value)
            {
                
                DisplayName = xElement.Element("DisplayName").Value,
                ValueType = int.TryParse(xElement.Element("ValueType").Value, out int valueType) ? valueType : 0,
                IsReadOnly = xElement.Element("IsReadOnly").Value =="True",
                DefaultValue = xElement.Element("DefaultValue").Value,
                IsRequired = xElement.Element("IsRequired").Value == "True",
                IsLimitedInput = xElement.Element("IsLimitedInput").Value == "True",
                ValueList = xElement.Element("ValueList").Value,
                MinValue = int.TryParse(xElement.Element("MinValue").Value, out int minValue) ? minValue : 0,
                MaxValue = int.TryParse(xElement.Element("MaxValue").Value, out int maxValue) ? maxValue : 0,
                ValidateStr = xElement.Element("ValidateStr").Value,
                ValidateErrTips = xElement.Element("ValidateErrTips").Value,
                BindingAction = xElement.Element("BindingAction").Value,
            };

            return property;
        }
        #endregion

        #region 退出时保存属性数据
        internal  void SaveProperties()
        {
            //string rootDirectory = "C:/Users/AideSoftware/source/repos/testTreeView/Resources/PropertyDefine.xml";
            string rootDirectory = ConstData.PropertyRootDirectory;
            XDocument xDocument = new XDocument();
            XElement rootElement = new XElement("Root");
            xDocument.Add(rootElement);
            SavePropertyDictToXml(rootElement);
            xDocument.Save(rootDirectory);
        }

        internal void SavePropertyDictToXml(XElement rootElement)
        {
            var keys = _partToPropertyDict.Keys;
            foreach( var key in keys )
            {
                XElement partElement = new XElement("Part", new XAttribute("code", key));
                rootElement.Add(partElement);
                foreach (Property  property in _partToPropertyDict[key].PropertyCollection)
                {
                    partElement.Add(PropertyToXElement(property));
                }
            }
        }

        internal XElement PropertyToXElement(Property property)
        {
            XElement xElement = new XElement("Property");
            xElement.Add(new XElement("Name", property.Name),
                new XElement("DisplayName", property.DisplayName),
                new XElement("ValueType", property.ValueType),
                new XElement("IsReadOnly", property.IsReadOnly),
                new XElement("DefaultValue", property.DefaultValue),
                new XElement("IsRequired", property.IsRequired),
                new XElement("IsLimitedInput", property.IsLimitedInput),
                new XElement("ValueList", property.ValueList),
                new XElement("MinValue", property.MinValue),
                new XElement("MaxValue", property.MaxValue),
                new XElement("ValidateStr", property.ValidateStr),
                new XElement("ValidateErrTips", property.ValidateErrTips),
                new XElement("BindingAction", property.BindingAction)
                );

            return xElement;
        }
        #endregion
    }
}
