using Crolow.Fast.Dawg.Dicos;
using System.IO.Compression;
namespace Crolow.Fast.Dawg.Base;

public class BaseCompiler
{
    public DawgNode Root { get; private set; }

    public DawgNodeBuild RootBuild { get; private set; }
    public int BuildNodeId { get; set; }


    private Dictionary<string, DawgNodeBuild> nodeCache;

    public BaseCompiler()
    {
        Root = new DawgNode();
        nodeCache = new Dictionary<string, DawgNodeBuild>();
    }

    public virtual void Insert(string word)
    {
    }


    public void Build(IEnumerable<string> words)
    {
        nodeCache = new Dictionary<string, DawgNodeBuild>();
        BuildNodeId = 0;
        RootBuild = new DawgNodeBuild();

        foreach (var word in words)
        {
            Insert(word);
        }

        Optimize();
    }

    private void Optimize()
    {
        int processedNodes = 0;

        List<DawgNodeBuild> nodesToProcess = new List<DawgNodeBuild> { RootBuild };
        while (nodesToProcess.Count > 0)
        {
            processedNodes++;
            var currentNode = nodesToProcess[0];
            nodesToProcess.RemoveAt(0);

            for (int x = 0; x < currentNode.Children.Count; x++)
            {
                var node = currentNode.Children[x];
                string nodeSignature = GetNodeSignature(node);

                if (nodeCache.ContainsKey(nodeSignature))
                {
                    currentNode.Children[x] = nodeCache[nodeSignature];
                }
                else
                {
                    nodeCache[nodeSignature] = node;
                    nodesToProcess.Add(node);
                }
            }
        }
    }

    private string GetNodeSignature(DawgNodeBuild node)
    {
        // Include the letter in the signature to ensure uniqueness
        var childrenSignatures = node.Children
            .OrderBy(c => c.Letter) // Ensure ordered processing
            .Select(child => GetNodeSignature(child))
            .ToList();

        // Create a combined signature that includes the node's letter and terminal status
        var signature = $"[{node.Letter}{node.Control}-{string.Join(",", childrenSignatures)}]";

        return signature;
    }

    /// <summary>
    /// Save to File save the dictionary on disk
    /// </summary>
    /// <param name="filePath"></param>
    public void SaveToFile(string filePath)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        using (GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Compress))
        using (BinaryWriter writer = new BinaryWriter(gzipStream))
        {
            Dictionary<DawgNodeBuild, int> nodeToId = new Dictionary<DawgNodeBuild, int>();
            int currentId = 0;

            List<DawgNodeBuild> writeOrder = new List<DawgNodeBuild>();
            CollectNodesWithIds(RootBuild, nodeToId, ref currentId, writeOrder);

            writer.Write(writeOrder.Count);

            foreach (var node in writeOrder)
            {
                WriteNodeWithId(node, writer, nodeToId);
            }
        }
    }

    private void CollectNodesWithIds(DawgNodeBuild node, Dictionary<DawgNodeBuild, int> nodeToId, ref int currentId, List<DawgNodeBuild> writeOrder)
    {
        if (nodeToId.ContainsKey(node))
            return;

        nodeToId[node] = currentId++;
        writeOrder.Add(node);

        foreach (var child in node.Children)
        {
            CollectNodesWithIds(child, nodeToId, ref currentId, writeOrder);
        }
    }

    private void WriteNodeWithId(DawgNodeBuild node, BinaryWriter writer, Dictionary<DawgNodeBuild, int> nodeToId)
    {
        writer.Write(node.Control);
        writer.Write((byte)node.Children.Count);

        foreach (var child in node.Children)
        {
            writer.Write(child.Letter); // Write the child letter
            writer.Write(nodeToId[child]);   // Write the child ID
        }
    }

    public void ReadFromFile(string filePath)
    {
        Root = new DawgNode();

        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
        using (GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
        using (BinaryReader reader = new BinaryReader(gzipStream))
        {
            int nodeCount = reader.ReadInt32();
            List<DawgNode> nodeList = new List<DawgNode>(nodeCount);

            for (int i = 0; i < nodeCount; i++)
            {
                nodeList.Add(new DawgNode());
            }

            for (int i = 0; i < nodeCount; i++)
            {
                ReadNodeWithId(nodeList[i], reader, nodeList);
            }

            Root = nodeList[0];
        }
    }

    private void ReadNodeWithId(DawgNode node, BinaryReader reader, List<DawgNode> nodeList)
    {
        node.Control = reader.ReadByte();
        int childrenCount = reader.ReadByte();

        for (int i = 0; i < childrenCount; i++)
        {
            byte childLetter = reader.ReadByte();
            int childId = reader.ReadInt32();

            DawgNode childNode = nodeList[childId];
            childNode.Letter = childLetter;  // Set the letter for the child node
            node.Children.Add(childNode);
        }
    }
}
