<script lang="ts">

	let title:string = "";
	let description:string = "";
	let location:string = "";
	let time:string = "";
	let errorStream:{error?:string} = {};
	let error = false;

	async function createPlan(e: Event) {
		e.preventDefault();
		const response = await fetch("/api/plan/create", {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
			body: JSON.stringify({ title, description, location, time }),
		});
		if (!response.ok) {
			errorStream = JSON.parse(await response.text());
			error = true;
		} else {
			const data = await response.json();
			localStorage.setItem("token", data.token);
			window.location.href = "/";
		}
	}
</script>

<div class="modal">
	<h2 class="modal-title">New Plan</h2>
	{#if error}
		<p class="error-text">{errorStream}</p>
	{/if}
	<form class="login-form" on:submit={createPlan} method="post">
		<input
			class="modal-input"
			type="text"
			bind:value={title}
			placeholder="Plan title"
		/>
		<input
			class="modal-input"
			type="text"
			bind:value={location}
			placeholder="Location"
		/>
		<input
			class="modal-input"
			type="datetime-local"
			bind:value={time}
			placeholder="time"
		/>
		<textarea
			class="modal-input"
			bind:value={description}
			placeholder="Enter plan description here.">
		</textarea>
		<button class="modal-btn" type="submit">Sign Up</button>
	</form>
</div>
