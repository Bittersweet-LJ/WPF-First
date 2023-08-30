using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using testTreeView.Models;

namespace testTreeView.Services
{
    public class LocationModeDefineService
    {

        private static readonly Dictionary<string, LocationList> _partToLocationDict = new Dictionary<string, LocationList>();

        static LocationModeDefineService()
        {
            InitPartToLocationDict();
        }
        public LocationModeDefineService()
        {
            
        }

        internal LocationList GetLocationListFromDict(PartType partType)
        {
           if( _partToLocationDict.TryGetValue(partType.Code, out LocationList locationList))
            {
                return locationList;
            }
            return null;
        }

        /// <summary>
        /// 新建零件加入定位方式字典
        /// </summary>
        /// <param name="partType"></param>
        internal static void AddPartToLocationDict(PartType partType)
        {
            _partToLocationDict.Add(partType.Code, new LocationList());
        }

        /// <summary>
        /// 从定位方式字典中删除零件
        /// </summary>
        /// <param name="partType"></param>
        /// <returns></returns>
        internal static bool DelPartFromLocationDict(PartType partType)
        {
            return  _partToLocationDict.Remove(partType.Code);
        } 

        /// <summary>
        /// 对选定零件创建定位方式
        /// </summary>
        /// <param name="partType"></param>
        /// <param name="locationName"></param>
        /// <returns></returns>
        internal Location CreateLocation(PartType partType,string locationName)
        {
            Location location = new Location(locationName);
            _partToLocationDict[partType.Code].LocationCollection.Add(location);
            return location;
        }
        /// <summary>
        /// 对选定零件删除定位方式
        /// </summary>
        /// <param name="partType"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        internal bool DelLocation(PartType partType,Location location)
        {
           return _partToLocationDict[partType.Code].LocationCollection.Remove(location);
        }

        #region 初始化定位数据
        private static void InitPartToLocationDict()
        {
            string rootDirectory = ConstData.LocationRootDirectory;
            if (File.Exists(rootDirectory))
            {
                XDocument xDocument = XDocument.Load(rootDirectory);
                XElement RootElement = xDocument.Root;
                LoadLocationListFromXMl(RootElement);
            }
            
        }

        private static  void LoadLocationListFromXMl(XElement element)
        {
            foreach(XElement partElement in element.Elements("Part"))
            {
                string key = partElement.Attribute("code").Value;
                LocationList locations = new LocationList();
                foreach(XElement locationElement in partElement.Elements("Location"))
                {
                    Location location = CreateLocationFromXElement(locationElement);
                    locations.LocationCollection.Add(location);
                }
                _partToLocationDict.Add(key, locations);
            }
        }
         private static  Location CreateLocationFromXElement(XElement element)
        {
            Location location = new Location(element.Element("Name")?.Value)
            {
                DisplayName = element.Element("DisplayName")?.Value,
                BasePartCode = element.Element("BasePartCode")?.Value,
                Mode = int.TryParse(element.Element("Mode")?.Value, out int result) ? result : 0,
                IsDefault = element.Attribute("isDefault")?.Value == "True",
            };
            return location;
        }


        #endregion

        #region 退出时保存定位数据
        internal void SaveLocations()
        {
            string rootDirectory = ConstData.LocationRootDirectory;
            XDocument xDocument = new XDocument();
            XElement rootElement = new XElement("Root");
            xDocument.Add(rootElement);
            SaveLocationDictToXml(rootElement);
            xDocument.Save(rootDirectory);
        }

        internal void SaveLocationDictToXml(XElement element)
        {
            foreach(string key in _partToLocationDict.Keys)
            {
                XElement partElement = new XElement("Part", new XAttribute("code",key));
                element.Add(partElement);
                foreach (Location location in _partToLocationDict[key].LocationCollection)
                {
                    XElement xElement = LocationToXElement(location);
                    partElement.Add(xElement);
                }
            }
        }

        internal XElement LocationToXElement(Location location)
        {
            XElement xElement = new XElement("Location", new XAttribute("isDefault", location.IsDefault.ToString()));
            xElement.Add(new XElement("Name", location.Name),
                new XElement("DisplayName",location.DisplayName),
                new XElement("BasePartCode", location.BasePartCode),
                new XElement("Mode", location.Mode)
                );
            return xElement;
        }
        #endregion
    }
}
