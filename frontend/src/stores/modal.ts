import { writable } from "svelte/store";

export const modalType = writable<string | null>(null);

export let modal = writable<boolean>(false);

export function displayModal(type: string) {
	modal.set(true);
	modalType.set(type);
}

export function closeModal() {
	modal.set(false);
}