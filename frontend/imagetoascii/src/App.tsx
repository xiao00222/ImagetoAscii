import { useState, type ChangeEvent } from "react";
import "./App.css";
import type { ConvertResponse } from "./types";

function App() {
  const [preview, setPreview] = useState<string | null>(null);
  const [ascii, setAscii] = useState<string>("");
  const [loading, setLoading] = useState<boolean>(false);
  const [fileName, setFileName] = useState<string>("");
  const handleFile = async (file: File | undefined) => {
    if (!file) return;
    setFileName(file.name);
    setPreview(URL.createObjectURL(file));
    setAscii("");
    setLoading(true);

    const formData = new FormData();
    formData.append("file", file);

    try {
      const res = await fetch("http://localhost:5225/api/convert", {
        method: "POST",
        body: formData,
      });
      const data: ConvertResponse = await res.json();
      setAscii(data.ascii);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const onFileInputChange = (e: ChangeEvent<HTMLInputElement>) => {
    handleFile(e.target.files?.[0]);
  };

  const downloadAscii = () => {
    const blob = new Blob([ascii], { type: "text/plain" });
    const url = URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.href = url;
    a.download = `${fileName.split(".")[0] || "ascii-art"}.txt`;
    a.click();
    URL.revokeObjectURL(url);
  };

  return (
    <div className="app">
      <h1>ASCII Artify</h1>
      <label className="dropzone">
        <input
          type="file"
          accept="image/*"
          hidden
          onChange={onFileInputChange}
        />
        {preview ? (
          <img src={preview} alt="preview" />
        ) : (
          <p>Click to upload an image</p>
        )}
      </label>

      {loading && <p className="status">Converting...</p>}

      {ascii && (
        <>
          <pre className="ascii-output">{ascii}</pre>
          <button onClick={downloadAscii}>Download .txt</button>
        </>
      )}
    </div>
  );
}
export default App;
