#pragma once

#ifdef PLUGIN_EXPORTS
#define PLUGIN_H __declspec(dllexport)
#elif PLUGIN_IMPORTS
#define PLUGIN_H __declspec(dllimport)
#else
#define PLUGIN_H
#endif

