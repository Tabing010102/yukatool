﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yuka.Script {
	class TextUtils {
		public static int defaultLineWidth = 50;
		public static string doubleWidthChars = "～~♪「」『』";

		public static int StringWidth(string word) {
			int len = word.Length;
			foreach(char ch in word) {
				if(doubleWidthChars.Contains(ch)) {
					len++;
				}
			}
			return len;
		}

		public static string Prefix(string word, int width) {
			double len = 0;
			for(int i = 0; i < word.Length; i++) {
				char cur = word[i];
				if(doubleWidthChars.Contains(cur)) {
					if(len + 1 == width) {
						// we'd have to cut the current char in half, so we omit it.
						return word.Substring(0, i);
					}
					len += 2.01;
				}
				else if(len >= width) {
					return word.Substring(0, i - 1);
				}
				else {
					len++;
				}
			}
			return word;
		}

		public static string Space(int length) {
			return new string(new char[length]).Replace('\0', ' ');
		}

		public static string WrapWords(string source) {
			return WrapWords(source, defaultLineWidth);
		}

		public static string WrapWords(string source, int lineWidth) {
			string[] words = source.Replace("\r", "").Replace("\\n", "\n").Split(' ');
			List<string> lines = new List<string>();
			StringBuilder line = new StringBuilder(lineWidth);
			int remaining = lineWidth;

			foreach(string w in words) {
				string word = w;
				int width = StringWidth(word);
				while(width > lineWidth) {
					// if the word wouldn't fit in the whole line, we keep it on the current one.
					string prefix = Prefix(word, remaining);
					int len = prefix.Length;
					word = word.Substring(len);
					width -= StringWidth(prefix);
					remaining -= len;
					line.Append(prefix);
					line.Append(Space(remaining));
					lines.Add(line.ToString());
					line.Clear();
					remaining = lineWidth;
				}

				if(width > remaining) {
					line.Append(Space(remaining));
					lines.Add(line.ToString());
					line.Clear().Append(word);
					remaining = lineWidth - width;
				}
				else {
					line.Append(word);
					remaining -= width;
				}

				if(remaining <= 1 || word.EndsWith("\n")) {
					line.Append(Space(remaining));
					lines.Add(line.ToString());
					line.Clear();
					remaining = lineWidth;
				}
				else {
					remaining--;
					line.Append(' ');
				}
			}
			lines.Add(line.ToString().TrimEnd());
			return string.Join("", lines);
		}

		public static string ReplaceSpecialChars(string source) {
			StringBuilder sb = new StringBuilder(source);

			// use ─ for seamless lines
			// don't use these note characters: ♩♫♬🎝🎵

			sb.Replace('（', '(');
			sb.Replace('）', ')');
			sb.Replace('『', '"');
			sb.Replace('』', '"');
			sb.Replace("「", "");
			sb.Replace("」", "");
			sb.Replace("、", ", ");
			sb.Replace("。", ". ");
			sb.Replace('！', '!');
			sb.Replace('？', '?');
			sb.Replace('~', '～');
			sb.Replace('⁓', '～');
			sb.Replace('–', '-');
			sb.Replace('—', '-');
			sb.Replace('ー', '-');
			sb.Replace('ｰ', '-');
			sb.Replace('―', '-');

			return sb.ToString();
		}

		public static string ProgressBar(int width, double progress) {
			string line = new string('=', (int)(width * progress)) + new string('.', width - (int)(width * progress));
			string text = progress.ToString("p");
			return line.Substring(0, (width - text.Length) / 2 - 1) + ' ' + text + ' ' + line.Substring((width + text.Length) / 2 + 2);
		}
	}
}
