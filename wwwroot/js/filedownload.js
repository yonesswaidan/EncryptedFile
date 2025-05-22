

// Den her funktion er til at downloade en fil i browseren ud fra dens navn og base64 indhold
function downloadFile(fileName, contentBase64) {
    const link = document.createElement('a');


    link.download = fileName;

    // Her sætter den href til en "data URL", som indeholder filens indhold kodet i base64
    link.href = "data:application/octet-stream;base64," + contentBase64;

    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
