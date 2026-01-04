using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace dominion.src.dominion.Dependencies.Data
{
    public sealed class InMemoryZip : IDisposable
    {
        public int Count => _entries.Count;

        private readonly ConcurrentDictionary<string, byte[]> _entries = new ConcurrentDictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);
        private readonly object _buildLock = new object();
        private bool _disposed = false;

        private static string NormalizeEntryName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Entry name is null or empty", nameof(name));

            name = name.Replace('\\', '/').Trim('/');

            if (name.Length != 0)
                return name;

            throw new ArgumentException("Invalid entry name", nameof(name));
        }

        public void AddFile(string entryPath, byte[] content)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(InMemoryZip));

            if (content != null && content.Length > 0)
            {
                string key = NormalizeEntryName(entryPath);
                byte[] copy = new byte[content.Length];
                Buffer.BlockCopy(content, 0, copy, 0, content.Length);

                _entries.AddOrUpdate(key, copy, (k, old) => copy);
            }
        }

        public void AddTextFile(string entryPath, string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                AddFile(entryPath, Encoding.UTF8.GetBytes(text));
            }
        }

        public void AddDirectoryFiles(string sourceDirectory, string targetEntryDirectory = "", bool recursive = true)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(InMemoryZip));

            if (string.IsNullOrEmpty(sourceDirectory))
                throw new ArgumentException("sourceDirectory is null or empty", nameof(sourceDirectory));

            if (!Directory.Exists(sourceDirectory))
                return;

            SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            foreach (string filePath in Directory.GetFiles(sourceDirectory, "*", searchOption))
            {
                string relativePath = filePath.Substring(sourceDirectory.Length)
                                              .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                string entryName = string.IsNullOrEmpty(targetEntryDirectory)
                    ? relativePath
                    : Path.Combine(targetEntryDirectory, relativePath);

                entryName = entryName.Replace('\\', '/');

                try
                {
                    byte[] content = File.ReadAllBytes(filePath);
                    AddFile(entryName, content);
                }
                catch
                {
                }
            }
        }

        public byte[] ToArray(CompressionLevel compression = CompressionLevel.Fastest)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(InMemoryZip));

            lock (_buildLock)
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true, Encoding.UTF8))
                    {
                        foreach (var kvp in _entries)
                        {
                            var entry = zipArchive.CreateEntry(kvp.Key, compression);
                            using (var stream = entry.Open())
                            {
                                stream.Write(kvp.Value, 0, kvp.Value.Length);
                            }
                        }
                    }

                    return memoryStream.ToArray();
                }
            }
        }

        public void Clear()
        {
            _entries.Clear();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                Clear();
            }
        }
    }
}