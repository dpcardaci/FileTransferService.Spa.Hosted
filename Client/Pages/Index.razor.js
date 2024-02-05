

window.validateOnBehalfOfInputText = (inputControlId) => {
    return document.getElementById(inputControlId).reportValidity();
}