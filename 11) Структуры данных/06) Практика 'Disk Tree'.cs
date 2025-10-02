using System;
using System.Collections.Generic;
using System.Linq;
namespace DiskTree
{
    public static class DiskTreeTask
    {
        public class FolderNode // 
        {
            public string Title { get; set; } // Foldername
            public SortedDictionary<string, FolderNode> SubFolders { get; } 
                = new SortedDictionary<string, FolderNode>(StringComparer.Ordinal); // Child вшкусе directories

            public FolderNode(string title)
            {
                Title = title; // Init\ folder name
            }
        }

		public static List<string> Solve(List<string> input) //
		{
			var rootFolder = new FolderNode(""); // Root

			foreach (var path in input)
			{
				var segments = path.Split('\\'); // Split
				var currentFolder = rootFolder; // root

				foreach (var segment in segments) // Traverse segments
				{
					if (!currentFolder.SubFolders.ContainsKey(segment)) //
					{
						currentFolder.SubFolders[segment] = new FolderNode(segment); //newfolder
					}
					currentFolder = currentFolder.SubFolders[segment]; // 
				}
			}

			var output = new List<string>(); // 
			ConstructTree(rootFolder, -1, output); // Build tree structure
			return output; // Ret
		}

        private static void ConstructTree(FolderNode node, int depth, List<string> output) // trbuilder
        {
            if (depth >= 0) // Cdepth
            {
                output.Add(new string(' ', depth) + node.Title); // Add folder name
            }

            foreach (var child in node.SubFolders.Values) // throught cjildren idk
            {
                ConstructTree(child, depth + 1, output); // Recursive call
            }
        }
    }
}