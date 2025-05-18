// Funktion til at downloade en fil i browseren ud fra deres navn og base64 indhold
function downloadFile(fileName, contentBase64) {
    const link = document.createElement('a');

    // Sætter navnet på den fil, der skal downloades
    link.download = fileName;

    // Sætter href til en "data URL", som indeholder filens indhold kodet i base64
    // MIME-typen 'application/octet-stream' bruges til at fortælle browseren, at det er binær data, som filen indeholder
    link.href = "data:application/octet-stream;base64," + contentBase64;

    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
