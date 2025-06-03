<script lang="ts">
	import { onMount } from "svelte";
	let { params } = $props();

	let plan: any = $state(null);
	let users: any[] = $state([]);


	onMount(async () => {
		// GET PLAN
		let response = await fetch(`/api/plan/${params.plancode}`, {
			method: "GET",
			headers: {
				Authorization: `Bearer ${localStorage.getItem("token")}`,
			},
		});
		if (response.ok) {
			plan = await response.json();
		}

		// GET PLAN PARTICIPANTS
		let response1 = await fetch(`/api/plan/get-users`, {
			method: "POST",
			headers: {
				Authorization: `Bearer ${localStorage.getItem("token")}`,
				"Content-Type": "application/json",
			},
			body: JSON.stringify(params.plancode),
		});
		if (response1.ok) {
			users = await response1.json();
			console.log(users);
		}
	});

	// EDIT PLAN
	const response = fetch(`/api/plan/${params.plancode}`, {
		method: "PUT",
		headers: {
			Authorization: `Bearer ${localStorage.getItem("token")}`,
			"Content-type": "application/json",
		},
		// body: JSON.stringify()
	})
</script>

{#if plan}
	<div class="plan-con">
		<h1>{plan.title}</h1>
		<strong>Time:</strong><p>{plan.time}</p>
		<strong>Location:</strong><p>{plan.location}</p>
		{#if plan.description}
			<strong>Description:</strong><p>{plan.description}</p>
		{/if}
		{#each users as user, i}
			<p>{user.name}</p>
		{/each}
	</div>



{/if}
