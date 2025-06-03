<!-- <script lang="ts">
	export let params;
	let username = params.username;
</script>
	

<p>Profile page</p> -->

<script lang="ts">
	let { params }: { params: { itemId: string } } = $props();
</script>

<h1>Welcome, {params.itemId}!</h1>
