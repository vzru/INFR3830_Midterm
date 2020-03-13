#pragma once

enum PacketType {
	// initialization connection
	INIT = 0,
	USER,
	// game state
	STATE,

	// Entity Data
	ENTITY
};