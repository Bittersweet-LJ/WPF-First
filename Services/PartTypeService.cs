using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using testTreeView.Models;


namespace testTreeView.Services
{
    public class PartTypeService
    {
        public PartTypeService()
        {
            InitPartTypeTree();
        }
        public static int CodeUsedErrCode = 1;
        public static int CodeContainsSpecialChar = 2;

        private static PartTypeTree _partTypeTree = new PartTypeTree();
        private readonly Dictionary<string, PartType> _partTypesDict = new Dictionary<string, PartType>();

        /// <summary>
        /// 获取零件集合实例对象
        /// </summary>
        /// <returns></returns>
        public PartTypeTree GetPartTypeTree()
        {
            if(_partTypeTree == null)
            {
                _partTypeTree = new PartTypeTree();
            }
            return _partTypeTree;
        }

        /// <summary>
        /// 将新建零件添加入字典
        /// </summary>
        /// <param name="partType"></param>
        public void AddPartTypeToDictionary(PartType partType)
        {
            _partTypesDict.Add(partType.Code, partType);
        }
        /// <summary>
        /// 删除零件时从字典移除
        /// </summary>
        /// <param name="partType"></param>
        public void DelPartTypefromDictionary(PartType partType)
        {
            _partTypesDict.Remove(partType.Code);
            foreach (PartType  part in partType.Children)
            {             
                DelPartTypefromDictionary(part);//recursion
            }
            
        }

        


        /// <summary>
        /// 新建零件类型Code是否合法
        /// </summary>
        /// <param name="typeCode"></param>
        /// <param name="errCode"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal bool NewPartTypeCodeCanUse(string typeCode, out int errCode)
        {
            if (_partTypesDict.ContainsKey(typeCode))
            {
                errCode = CodeUsedErrCode;
                return false;
            }
            if (string.IsNullOrWhiteSpace(typeCode))
            {
                throw new ArgumentNullException(nameof(typeCode));
            }
            if (!Regex.IsMatch(typeCode, "^[a-zA-Z][a-zA-Z0-9_]*$"))
            {
                errCode = CodeContainsSpecialChar;
                return false;
            }
            errCode = 0;
            return true;
        }

        #region 零件类型增删
        /// <summary>
        /// 创建新零件类型
        /// </summary>
        /// <param name="typeCode"></param>
        /// <param name="parentPartType"></param>
        /// <returns></returns>
        internal PartType CreatePartType(string typeCode, PartType parentPartType)
        {
            PartType newPartType = new PartType(typeCode, parentPartType);
            if (parentPartType != null)
            {
                parentPartType.Children.Add(newPartType);
            }
            else
            {
                _partTypeTree.RootPartTypeCollection.Add(newPartType);
            }

            //存入各项字典
            AddPartTypeToDictionary(newPartType);
            PropertyDefineService.AddPartToPropertyDict(newPartType);
            LocationModeDefineService.AddPartToLocationDict(newPartType);
            return newPartType;
        }

        /// <summary>
        /// 删除零件类型
        /// </summary>
        /// <param name="partType"></param>
        internal void DeletePartType(PartType partType)
        {

            if (_partTypeTree.RootPartTypeCollection.Contains(partType))
            {
                _ = _partTypeTree.RootPartTypeCollection.Remove(partType);
            }
            else
            {
                _ = partType.Parent.Children.Remove(partType);
            }


            //递归所有子零件,从各字典删除
            DeletePartAllChildFromDict(partType);


        }

        internal void DeletePartAllChildFromDict(PartType partType)
        {
            DelPartTypefromDictionary(partType);
            PropertyDefineService.DelPartFromPropertyDict(partType);
            LocationModeDefineService.DelPartFromLocationDict(partType);
            foreach (PartType part in partType.Children)
            {
                DeletePartAllChildFromDict(part);
            }
        }

        #endregion

        #region 程序启动时初始化零件树数据
        internal void InitPartTypeTree()
        {
            //内容文件
            //string rootDirectory = "Resources/PartTypes.xml"; // Todo:System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase 
            //string rootDirectory = "C:/Users/AideSoftware/source/repos/testTreeView/Resources/PartTypes.xml";
            string rootDirectory = ConstData.PartRootDirectory;

            if (File.Exists(rootDirectory))
            {
                XDocument xDocument = XDocument.Load(rootDirectory);
                XElement rootElement = xDocument.Element("parts");
                _partTypeTree.RootPartTypeCollection = LoadXmlToPartTypes(rootElement, null);
            }
              
        }


        private ObservableCollection<PartType> LoadXmlToPartTypes(XElement rootElement, PartType parentPartType)
        {
            ObservableCollection<PartType> partTypes = new ObservableCollection<PartType>();
            foreach (XElement element in rootElement.Elements())
            {
                PartType partType = CreatePartFromXElement(element, parentPartType);

                AddPartTypeToDictionary(partType);              

                partType.Children = LoadXmlToPartTypes(element, partType);
                partTypes.Add(partType);
            }
            return partTypes;
        }

        internal PartType CreatePartFromXElement(XElement element, PartType parentPartType)
        {
            
            PartType partType = new PartType(element.Attribute("code")?.Value, parentPartType)
            {
                DisplayName = element.Attribute("displayName")?.Value,
                Icon = element.Attribute("icon")?.Value,
                CodeRules = element.Attribute("codeRules")?.Value,
                NameRules = element.Attribute("nameRules")?.Value,
                IsReplaceable = element.Attribute("isReplaceable")?.Value == "True",
                IsAssembleable = element.Attribute("isAssembleable")?.Value == "True",
            };
            return partType;
        }

        #endregion

        #region 程序退出保存数据
        internal void SavePartTypes()
        {
            XDocument xDoc = new XDocument();
            XElement rootElement = new XElement("parts");
            xDoc.Add(rootElement);
            SavePartTypesToXml(_partTypeTree.RootPartTypeCollection, rootElement);
            //内容文件
            //xDoc.Save("Resources/PartTypes.xml");
            //xDoc.Save("C:/Users/AideSoftware/source/repos/testTreeView/Resources/PartTypes.xml");
            xDoc.Save(ConstData.PartRootDirectory);

        }

        internal void SavePartTypesToXml(ObservableCollection<PartType> partTypes, XElement parentElement)
        {

            foreach (PartType partType in partTypes)
            {
                //XElement element = new XElement("item", new XAttribute("code", partType.Code));
                XElement element = PartTypeToXElement(partType);
                parentElement.Add(element);
                SavePartTypesToXml(partType.Children, element);
            }        
        }


        internal XElement PartTypeToXElement(PartType partType)
        {
            XElement xElement = new XElement("item",
                new XAttribute("code", partType.Code),
                new XAttribute("displayName", partType.DisplayName ?? ""),
                new XAttribute("icon", partType.Icon ?? ""),
                new XAttribute("codeRules", partType.CodeRules ?? ""),
                new XAttribute("nameRules", partType.NameRules ?? ""),
                new XAttribute("isReplaceable", partType.IsReplaceable.ToString()),
                new XAttribute("isAssembleable", partType.IsAssembleable.ToString())
                );

            return xElement;
        }
        #endregion

    }


}
