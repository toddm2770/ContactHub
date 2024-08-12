export function initToast(toastEL) {
    const toast = bootstrap.Toast.getOrCreateInstance(toastEL);
    toast.show();
}