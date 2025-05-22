import { writable } from "svelte/store";

export let token = writable<Record<string, any> | null>(
	decodeToken(localStorage.getItem("token"))
);

function decodeToken(token: string | null): any {
	if (token && token !== "undefined") {
		try {
			const base64 = token.split(".")[1].replace(/-/g, "+").replace(/_/g, "/");
			const jsonPayload = decodeURIComponent(
				atob(base64)
					.split("")
					.map(function (c) {
						return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
					})
					.join("")
			);
			return JSON.parse(jsonPayload);
		} catch {
			console.log("invalid token");
			return null;
		}
	} else {
		return null;
	}
}

export function setToken(t: string | null) {
	token.set(decodeToken(t));
	if (t) localStorage.setItem("token", t);
}
