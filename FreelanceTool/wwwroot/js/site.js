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

// Select language handler
(function () {
	//$("#selectLanguage select").change(function () {
	//	$(this).parent().submit();
	//});
	$(".language-select-provider").on("click",
		function () {

			var currentCulture = $("#language-select-current-culture").val();
			var culture = $(this).data("culture-name");
			if (currentCulture === culture) return;

			$("input[name='culture']").first().val(culture);
			$("form#language-select-form").submit();
		});
}());

// Logout handler
(function() {
	$("#logoutButton").on("click",
		function() {
			$("form#logoutForm").submit();
		});
}());