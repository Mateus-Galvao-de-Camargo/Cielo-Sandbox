function startLoading(buttonId) {
    const button = document.getElementById(buttonId);
    if (button) {
        button.disabled = true;
        button.innerHTML = 'Processando...';
    }
    console.log('Iniciando processamento...');
}

function stopLoading(buttonId) {
    const button = document.getElementById(buttonId);
    if (button) {
        button.disabled = false;
        button.innerHTML = 'Adicionar';
    }
}