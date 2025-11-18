using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Excel;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System;

public class ExcelUtility
{

	/// <summary>
	/// 表格数据集合
	/// </summary>
	private DataSet mResultSet;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="excelFile">Excel file.</param>
	public ExcelUtility (string excelFile)
	{
		FileStream mStream = File.Open (excelFile, FileMode.Open, FileAccess.Read);
		IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader (mStream);
		mResultSet = mExcelReader.AsDataSet ();
	}
			
	/// <summary>
	/// 转换为实体类列表
	/// </summary>
	public List<T> ConvertToList<T> ()
	{
		//判断Excel文件中是否存在数据表
		if (mResultSet.Tables.Count < 1)
			return null;
		//默认读取第一个数据表
		DataTable mSheet = mResultSet.Tables [0];
			
		//判断数据表内是否存在数据
		if (mSheet.Rows.Count < 1)
			return null;

		//读取数据表行数和列数
		int rowCount = mSheet.Rows.Count;
		int colCount = mSheet.Columns.Count;
				
		//准备一个列表以保存全部数据
		List<T> list = new List<T> ();
				
		//读取数据
		for (int i=1; i<rowCount; i++) 
		{
			//创建实例
			Type t = typeof(T);
			ConstructorInfo ct = t.GetConstructor (System.Type.EmptyTypes);
			T target = (T)ct.Invoke (null);
			for (int j=0; j<colCount; j++) 
			{
				//读取第1行数据作为表头字段
				string field = mSheet.Rows [0] [j].ToString ();
				object value = mSheet.Rows [i] [j];
				//设置属性值
				SetTargetProperty (target, field, value);
			}
					
			//添加至列表
			list.Add (target);
		}
				
		return list;
	}

    /// <summary>
    /// 转换为Json，支持方括号数组
    /// </summary>
    /// <param name="JsonPath">Json文件路径</param>
    /// <param name="encoding">编码</param>
    public void ConvertToJson(string JsonPath, Encoding encoding)
    {
        if (mResultSet.Tables.Count < 1)
            return;

        DataTable mSheet = mResultSet.Tables[0];
        if (mSheet.Rows.Count < 1)
            return;

        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;

        List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();

        // 从第 1 行开始（跳过表头）
        for (int i = 1; i < rowCount; i++)
        {
            Dictionary<string, object> row = new Dictionary<string, object>();

            for (int j = 0; j < colCount; j++)
            {
                string field = mSheet.Rows[0][j].ToString();
                object value = mSheet.Rows[i][j];

                string strValue = value?.ToString().Trim() ?? "";

                // --- 支持方括号数组 ---
                if (strValue.StartsWith("[") && strValue.EndsWith("]"))
                {
                    string inner = strValue.Substring(1, strValue.Length - 2).Trim();

                    if (string.IsNullOrEmpty(inner))
                    {
                        row[field] = new Newtonsoft.Json.Linq.JArray();
                    }
                    else
                    {
                        bool isStringArray = inner.Contains("\"") || inner.Contains("'");

                        string[] parts = inner.Split(',');
                        for (int k = 0; k < parts.Length; k++)
                            parts[k] = parts[k].Trim(' ', '"', '\'');

                        // 数字数组尝试解析为 int/double
                        if (!isStringArray)
                        {
                            List<object> numList = new List<object>();
                            foreach (var p in parts)
                            {
                                string t = p.Trim();
                                if (int.TryParse(t, out int iv))
                                    numList.Add(iv);
                                else if (double.TryParse(t, out double dv))
                                    numList.Add(dv);
                                else
                                    numList.Add(t); // fallback
                            }
                            row[field] = new Newtonsoft.Json.Linq.JArray(numList);
                        }
                        else
                        {
                            row[field] = new Newtonsoft.Json.Linq.JArray(parts);
                        }
                    }
                }
                else
                {
                    // --- 普通值 ---
                    if (value is double d)
                    {
                        if (d == Math.Floor(d))
                            value = (int)d;
                        else
                            value = d;
                    }

                    // 布尔值处理
                    string str = strValue.ToLower();
                    if (str == "true") value = true;
                    else if (str == "false") value = false;

                    row[field] = value;
                }
            }

            table.Add(row);
        }

        // --- 写入 JSON ---
        using (FileStream fileStream = new FileStream(JsonPath, FileMode.Create, FileAccess.Write))
        using (StreamWriter textWriter = new StreamWriter(fileStream, encoding))
        using (JsonTextWriter writer = new JsonTextWriter(textWriter))
        {
            writer.Formatting = Formatting.Indented; // 顶层对象缩进
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(writer, table);
        }
    }


    // 自定义 JsonTextWriter：对象缩进，数组单行
    class SingleLineArrayJsonWriter : JsonTextWriter
    {
        public SingleLineArrayJsonWriter(TextWriter textWriter) : base(textWriter)
        {
            Formatting = Formatting.Indented; // 保持对象缩进
        }

        public override void WriteStartArray()
        {
            base.WriteStartArray();
            Formatting = Formatting.None; // 数组里不缩进
        }

        public override void WriteEndArray()
        {
            base.WriteEndArray();
            Formatting = Formatting.Indented; // 结束数组后恢复缩进
        }
    }

    /// <summary>
    /// 转换为CSV
    /// </summary>
    public void ConvertToCSV (string CSVPath, Encoding encoding)
	{
		//判断Excel文件中是否存在数据表
		if (mResultSet.Tables.Count < 1)
			return;

		//默认读取第一个数据表
		DataTable mSheet = mResultSet.Tables [0];

		//判断数据表内是否存在数据
		if (mSheet.Rows.Count < 1)
			return;

		//读取数据表行数和列数
		int rowCount = mSheet.Rows.Count;
		int colCount = mSheet.Columns.Count;

		//创建一个StringBuilder存储数据
		StringBuilder stringBuilder = new StringBuilder ();

		//读取数据
		for (int i = 0; i < rowCount; i++) {
			for (int j = 0; j < colCount; j++) {
				//使用","分割每一个数值
				stringBuilder.Append (mSheet.Rows [i] [j] + ",");
			}
			//使用换行符分割每一行
			stringBuilder.Append ("\r\n");
		}

		//写入文件
		using (FileStream fileStream = new FileStream(CSVPath, FileMode.Create, FileAccess.Write)) {
			using (TextWriter textWriter = new StreamWriter(fileStream, encoding)) {
				textWriter.Write (stringBuilder.ToString ());
			}
		}

	}

	/// <summary>
	/// 导出为Xml
	/// </summary>
	public void ConvertToXml (string XmlFile)
	{
		//判断Excel文件中是否存在数据表
		if (mResultSet.Tables.Count < 1)
			return;

		//默认读取第一个数据表
		DataTable mSheet = mResultSet.Tables [0];

		//判断数据表内是否存在数据
		if (mSheet.Rows.Count < 1)
			return;

		//读取数据表行数和列数
		int rowCount = mSheet.Rows.Count;
		int colCount = mSheet.Columns.Count;

		//创建一个StringBuilder存储数据
		StringBuilder stringBuilder = new StringBuilder ();
		//创建Xml文件头
		stringBuilder.Append ("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
		stringBuilder.Append ("\r\n");
		//创建根节点
		stringBuilder.Append ("<Table>");
		stringBuilder.Append ("\r\n");
		//读取数据
		for (int i = 1; i < rowCount; i++) {
			//创建子节点
			stringBuilder.Append ("  <Row>");
			stringBuilder.Append ("\r\n");
			for (int j = 0; j < colCount; j++) {
				stringBuilder.Append ("   <" + mSheet.Rows [0] [j].ToString () + ">");
				stringBuilder.Append (mSheet.Rows [i] [j].ToString ());
				stringBuilder.Append ("</" + mSheet.Rows [0] [j].ToString () + ">");
				stringBuilder.Append ("\r\n");
			}
			//使用换行符分割每一行
			stringBuilder.Append ("  </Row>");
			stringBuilder.Append ("\r\n");
		}
		//闭合标签
		stringBuilder.Append ("</Table>");
		//写入文件
		using (FileStream fileStream = new FileStream(XmlFile, FileMode.Create, FileAccess.Write)) {
			using (TextWriter textWriter = new StreamWriter(fileStream,Encoding.GetEncoding("utf-8"))) {
				textWriter.Write (stringBuilder.ToString ());
			}
		}
	}

	/// <summary>
	/// 设置目标实例的属性
	/// </summary>
	private void SetTargetProperty (object target, string propertyName, object propertyValue)
	{
		//获取类型
		Type mType = target.GetType ();
		//获取属性集合
		PropertyInfo[] mPropertys = mType.GetProperties ();
		foreach (PropertyInfo property in mPropertys) {
			if (property.Name == propertyName) {
				property.SetValue (target, Convert.ChangeType (propertyValue, property.PropertyType), null);
			}
		}
	}
}

