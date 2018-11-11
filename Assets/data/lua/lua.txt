
require("a")

lua = {}

function awake()
	print("lua awake...")
	print(a.print())
end

function start(args)
	print("lua start..."..args)
end

function update(time)
	a.update(time)
	--print("lua update...")
end

return lua