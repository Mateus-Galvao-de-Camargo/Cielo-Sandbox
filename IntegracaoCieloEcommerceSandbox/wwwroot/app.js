function startLoading(buttonId) {
  const button = document.getElementById(buttonId);
  if (button) {
    button.disabled = true;
    const originalText = button.innerHTML;
    button.innerHTML =
      '<span class="spinner-border me-2" role="status" aria-hidden="true"></span><span>Processando...</span>';
    button.dataset.originalHTML = originalText;
  }
}

function stopLoading(buttonId) {
  const button = document.getElementById(buttonId);
  if (button) {
    button.disabled = false;
    button.innerHTML = button.dataset.originalHTML || "Enviar";
    delete button.dataset.originalHTML;
  }
}

document.addEventListener("submit", function (e) {
  const form = e.target;
  if (!(form instanceof HTMLFormElement)) return;
  const button = form.querySelector('button[type="submit"]');
  if (button && button.id) {
    startLoading(button.id);
  }
});
