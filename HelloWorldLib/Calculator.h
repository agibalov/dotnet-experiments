#pragma once

using namespace System;

namespace HelloWorldLib {
	public ref class Calculator
	{
	public: 
		Int32 InstanceAddNumbers(Int32 a, Int32 b);
		static Int32 StaticAddNumbers(Int32 a, Int32 b);
	};
}
