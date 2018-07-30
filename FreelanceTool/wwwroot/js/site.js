function getChosenFileName(filePath) {
	var filename = null;
	if (filePath) {
		var startIndex = (filePath.indexOf('\\') >= 0 ?
			filePath.lastIndexOf('\\') : filePath.lastIndexOf('/'));
		filename = filePath.substring(startIndex);
		if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
			filename = filename.substring(1);
		}
	}

	return filename;
}

function isFileExtensionValid(filePath, fileValidExtensions) {

	// No file was chosen, no need to validate extension.
	if (filePath.length <= 0) return true;

	var isExtensionValid = false;
	for (var i = 0; i < fileValidExtensions.length; i++) {
		var currentAllowedExtension = fileValidExtensions[i].toLowerCase();
		var currentExtensionLength = currentAllowedExtension.length;
		var fileExtension = filePath
			.substring(filePath.length - currentExtensionLength)
			.toLowerCase();

		if (fileExtension === currentAllowedExtension) {
			isExtensionValid = true;
			break;
		}
	}

	return isExtensionValid;
}

function isFieldEmpty(inputField) {
	return inputField.val() === "";
}

function updateErrorMessage(inputField, errorSpanId) {
	var errorSpan = $(errorSpanId);
	if (isFieldEmpty(inputField))
		errorSpan.text(inputField.data("val-required-cond"));
	else
		errorSpan.text("");
}

function scrollToError(elementId) {
	$('html, body').animate({
		scrollTop: ($(elementId).offset().top)
	}, 200);
}