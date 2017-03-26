Main_Test = {}
local  __Me = Main_Test

--==================================================


function __Me.OnWaitForCall()
	UIUtils.Log('====== OnWaitForCall ============')
end


function __Me.OnEnable(sender)
	UIUtils.Log('=== first call by lifeEvent Enable =======')
	UIUtils.CatchEvent()
end

function __Me.OnDisable(sender)
	UIUtils.Log('=== first call by lifeEvent OnDisable =======')
	UIUtils.CatchEvent()
end

local timer = nil
function __Me.OnStopTimer()
	if timer ~= nil then
		timer:Stop()
	end
end

function __Me.BtnOnClick(sender)
	UIUtils.Log('=== first call by lifeEvent BtnOnClick =======')
	local lit = UIUtils.GetLit('man.text')
	UIUtils.Log(lit)
	timer = UIUtils.RepeatedCall(lit,'OnWaitForCall',1)
	UIUtils.WaitFor(lit, 'OnStopTimer', 10)
	UIUtils.CatchEvent()
end