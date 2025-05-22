import { writable } from "svelte/store";

export const modalType = writable<string | null>(null);