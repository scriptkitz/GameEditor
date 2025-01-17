﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
public class FloatParameter
{
	public float mValue;
	public string mComment;
	public GAME_DEFINE_FLOAT mType;
	public string mTypeString;
};
public class StringParameter
{
	public string mValue;
	public string mComment;
	public GAME_DEFINE_STRING mType;
	public string mTypeString;
};
public class ConfigInfo
{
	public string mComment;
	public string mName;
	public string mValue;
}
public class ConfigBase : FrameComponent
{
	public Dictionary<string, GAME_DEFINE_FLOAT> mFloatNameToDefine;
	public Dictionary<GAME_DEFINE_FLOAT, string> mFloatDefineToName;
	public Dictionary<string, GAME_DEFINE_STRING> mStringNameToDefine;
	public Dictionary<GAME_DEFINE_STRING, string> mStringDefineToName;
	protected Dictionary<GAME_DEFINE_FLOAT, FloatParameter> mFloatList;
	protected Dictionary<GAME_DEFINE_STRING, StringParameter> mStringList;
	protected bool mFloatOrString;
	protected string mOtherFileName;
	public ConfigBase(string name)
		: base(name)
	{
		mFloatNameToDefine = new Dictionary<string, GAME_DEFINE_FLOAT>();
		mFloatDefineToName = new Dictionary<GAME_DEFINE_FLOAT, string>();
		mStringNameToDefine = new Dictionary<string, GAME_DEFINE_STRING>();
		mStringDefineToName = new Dictionary<GAME_DEFINE_STRING, string>();
		mFloatList = new Dictionary<GAME_DEFINE_FLOAT, FloatParameter>();
		mStringList = new Dictionary<GAME_DEFINE_STRING, StringParameter>();
		mFloatOrString = true;
		mOtherFileName = "";
	}
	public override void init()
	{
		;
	}
	public override void destroy()
	{
		base.destroy();
	}
	// 配置本地 和临时文件信息 并读取
	public void mergeConfig(string thisFileName, string otherFileName)
	{
		mOtherFileName = otherFileName;
		readFile(thisFileName);
		ConfigBase tempConfig = createInstance<ConfigBase>(GetType(), mName);
		tempConfig.init();
		tempConfig.readFile(otherFileName);
		applyConfig(tempConfig);
	}
	public static T createInstance<T>(Type classType, params object[] param) where T : class
	{
		return Activator.CreateInstance(classType, param) as T;
	}
	public void applyConfig(ConfigBase tempConfig)
	{
		if(mFloatOrString)
		{
			foreach (var item in tempConfig.getFloatList())
			{
				float value = getFloatParam(item.Key);
				string comment = getFloatComment(item.Key);
				// 如果旧表里有键 那么保留目前的值到新表
				if (hasParameter(item.Key))
				{
					tempConfig.setFloatParam(item.Key, value, comment);
				}
				// 如果旧表里有缺少的键 那么就拿新表的键合并
				else
				{
					value = tempConfig.getFloatParam(item.Key);
					comment = tempConfig.getFloatComment(item.Key);
					tempConfig.setFloatParam(item.Key, value, comment);
				}
			}
			FileUtility.writeTxtFile(mOtherFileName, tempConfig.generateFloatFile());
		}
		else
		{
			foreach (var item in tempConfig.getStringList())
			{
				string value = getStringParam(item.Key);
				string comment = getStringComment(item.Key);
				if (hasParameter(item.Key))
				{
					tempConfig.setStringParam(item.Key, value, comment);
				}
				else
				{
					value = tempConfig.getStringParam(item.Key);
					comment = tempConfig.getStringComment(item.Key);
					tempConfig.setStringParam(item.Key, value, comment);
				}
			}
			FileUtility.writeTxtFile(mOtherFileName, tempConfig.generateStringFile());
		}
	}
	public float getFloatParam(GAME_DEFINE_FLOAT param)
	{
		if (mFloatList.ContainsKey(param))
		{
			return mFloatList[param].mValue;
		}

		return 0.0f;
	}
	public string getStringParam(GAME_DEFINE_STRING param)
	{
		if (mStringList.ContainsKey(param))
		{
			return mStringList[param].mValue;
		}
		return "";
	}
	public string getFloatComment(GAME_DEFINE_FLOAT param)
	{
		if (mFloatList.ContainsKey(param))
		{
			return mFloatList[param].mComment;
		}
		return "";
	}
	public string getStringComment(GAME_DEFINE_STRING param)
	{
		if (mStringList.ContainsKey(param))
		{
			return mStringList[param].mComment;
		}
		return "";
	}
	public void setFloatParam(GAME_DEFINE_FLOAT param, float value, string comment = "")
	{
		if (!mFloatList.ContainsKey(param))
		{
			FloatParameter floatParam = new FloatParameter();
			floatParam.mValue = value;
			floatParam.mComment = comment;
			floatParam.mType = param;
			floatParam.mTypeString = param.ToString();
			mFloatList.Add(param, floatParam);
		}
		else
		{
			mFloatList[param].mValue = value;
			if (comment != "")
			{
				mFloatList[param].mComment = comment;
			}
		}
	}
	public void setStringParam(GAME_DEFINE_STRING param, string value, string comment = "")
	{
		if (!mStringList.ContainsKey(param))
		{
			StringParameter strParam = new StringParameter();
			strParam.mValue = value;
			strParam.mComment = comment;
			strParam.mType = param;
			strParam.mTypeString = param.ToString();
			mStringList.Add(param, strParam);
		}
		else
		{
			mStringList[param].mValue = value;
			if (comment != "")
			{
				mStringList[param].mComment = comment;
			}
		}
	}
	public Dictionary<GAME_DEFINE_FLOAT, FloatParameter> getFloatList() { return mFloatList; }
	public Dictionary<GAME_DEFINE_STRING, StringParameter> getStringList() { return mStringList; }
	public void addFloatParam(GAME_DEFINE_FLOAT type)
	{
		mFloatNameToDefine.Add(type.ToString(), type);
		mFloatDefineToName.Add(type, type.ToString());
	}
	public void addStringParam(GAME_DEFINE_STRING type)
	{
		mStringNameToDefine.Add(type.ToString(), type);
		mStringDefineToName.Add(type, type.ToString());
	}
	//----------------------------------------------------------------------------------------------------------------------------------------------

	protected string floatTypeToName(GAME_DEFINE_FLOAT type)
	{
		if (mFloatDefineToName.ContainsKey(type))
		{
			return mFloatDefineToName[type];
		}
		return "";
	}
	protected GAME_DEFINE_FLOAT floatNameToType(string name)
	{
		if (mFloatNameToDefine.ContainsKey(name))
		{
			return mFloatNameToDefine[name];
		}
		return GAME_DEFINE_FLOAT.GDF_NONE;
	}
	protected string stringTypeToName(GAME_DEFINE_STRING type)
	{
		if (mStringDefineToName.ContainsKey(type))
		{
			return mStringDefineToName[type];
		}
		return "";
	}
	protected GAME_DEFINE_STRING stringNameToType(string name)
	{
		if (mStringNameToDefine.ContainsKey(name))
		{
			return mStringNameToDefine[name];
		}
		return GAME_DEFINE_STRING.GDS_NONE;
	}
	protected bool hasParameter(GAME_DEFINE_FLOAT param)
	{
		return mFloatList.ContainsKey(param);
	}
	protected bool hasParameter(GAME_DEFINE_STRING param)
	{
		return mStringList.ContainsKey(param);
	}
	public string generateFloatFile()
	{
		string preString = "// 注意\r\n// 每个参数上一行必须是该参数的注释\r\n// 可以添加任意的换行和空格\r\n// 变量命名应与代码中枚举命名相同\r\n\r\n";
		string nextLineStr = "\r\n\r\n";
		string fileString = preString;
		foreach (var info in mFloatList)
		{
			fileString += "// ";
			fileString += info.Value.mComment;
			fileString += "\r\n";
			fileString += info.Value.mTypeString;
			fileString += " = ";
			fileString += StringUtility.floatToString(info.Value.mValue, 2);
			fileString += nextLineStr;
		}
		// 移除最后的\r\n\r\n
		fileString = fileString.Substring(0, fileString.Length - nextLineStr.Length);
		return fileString;
	}
	public string generateStringFile()
	{
		string preString = "// 注意\r\n// 每个参数上一行必须是该参数的注释\r\n// 可以添加任意的换行和空格\r\n// 变量命名应与代码中枚举命名相同\r\n\r\n";
		string nextLineStr = "\r\n\r\n";
		string fileString = preString;
		foreach (var info in mStringList)
		{
			fileString += "// ";
			fileString += info.Value.mComment;
			fileString += "\r\n";
			fileString += info.Value.mTypeString;
			fileString += " = ";
			fileString += info.Value.mValue;
			fileString += nextLineStr;
		}
		// 移除最后的\r\n\r\n
		fileString = fileString.Substring(0, fileString.Length - nextLineStr.Length);
		return fileString;
	}
	public void readFile(string fileName)
	{
		string text = FileUtility.openTxtFile(fileName);
		string[] lineList = StringUtility.split(text, true, "\r\n");
		Dictionary<string, ConfigInfo> valueList = new Dictionary<string, ConfigInfo>();
		string comment = "";
		// 前4行需要被丢弃
		int dropLine = 4;
		for (int i = 0; i < lineList.Length; ++i)
		{
			if (i < dropLine)
			{
				continue;
			}
			string line = lineList[i];
			// 去除所有空白字符
			line = Regex.Replace(line, @"\s", "");
			// 如果该行是空的,或者是注释,则不进行处理
			if (line.Length > 0)
			{
				if (line.Substring(0, 2) == "//")
				{
					comment = line.Substring(2, line.Length - 2);
				}
				else
				{
					string[] value = StringUtility.split(line, false, "=");
					if (value.Length != 2)
					{
						logError("配置文件错误 : line : " + line);
						return;
					}
					ConfigInfo info = new ConfigInfo();
					info.mComment = comment;
					info.mName = value[0];
					info.mValue = value[1];
					if (!valueList.ContainsKey(info.mName))
					{
						valueList.Add(info.mName, info);
					}
				}
			}
		}

		List<string> keys = new List<string>(valueList.Keys);
		List<ConfigInfo> values = new List<ConfigInfo>(valueList.Values);
		for (int i = 0; i < keys.Count; ++i)
		{
			if (mFloatOrString)
			{
				GAME_DEFINE_FLOAT def = floatNameToType(keys[i]);
				if (def != GAME_DEFINE_FLOAT.GDF_NONE)
				{
					setFloatParam(def, StringUtility.stringToFloat(values[i].mValue), values[i].mComment);
				}
			}
			else
			{
				GAME_DEFINE_STRING def = stringNameToType(keys[i]);
				if (def != GAME_DEFINE_STRING.GDS_NONE)
				{
					setStringParam(def, values[i].mValue, values[i].mComment);
				}
			}
		}
	}
}