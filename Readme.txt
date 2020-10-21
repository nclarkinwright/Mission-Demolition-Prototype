The 3 implemented additionals are:

	Tracking of high score per level, written into MissionDemolition.cs.

	Showing all Projectile trails per level. Done by removing projectileLine, and adding a TrailRenderer
	to Projectile prefab. Set time to Infinity and turned emitting off. Activate emitting at Projectile
	launch.

	Implemented a "wood" block and slab, which are lighter and break into pieces when Projectile, and only
	Projectile, collides with them. The Projectile must be moving at a velocity greater than 10 Unity units
	per second. Destruction removes slab/block and replaces it with broken pieces, made as prefabs. Broken piece
	and number of broken pieces at spawn can be set in inspector. Broken pieces cleaned between levels with tag system.