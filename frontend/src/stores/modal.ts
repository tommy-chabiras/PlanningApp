import { writable } from "svelte/store";

export const modalType = writable<string | null>(null);

export let modal = writable<boolean>(false);

const modalCompleteResolver = writable<(() => void) | null>(null);


export function displayModal(type: string) {
	modal.set(true);
	modalType.set(type);

	return new Promise<void>((resolve) => {
		modalCompleteResolver.set(() => {
			modal.set(false);
			resolve();
		});
	});
}

export function completeModal() {
	modalCompleteResolver.update((fn) => {
		if (fn) fn();
		return null;
	})
}

