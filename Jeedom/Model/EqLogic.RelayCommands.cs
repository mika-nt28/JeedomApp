using Jeedom.Mvvm;
using System;
using System.Linq;

namespace Jeedom.Model
{
    public partial class EqLogic
    {
        /// <summary>
        /// Exécute une commande à partir de son "logicalId"
        /// </summary>
        public RelayCommand<object> ExecCommandByLogicalID
        {
            get
            {
                this._ExecCommandByLogicalID = this._ExecCommandByLogicalID ?? new RelayCommand<object>(async parameters =>
                {
                    var cmd = Cmds.Where(c => c.LogicalId.ToLower() == parameters.ToString().ToLower()).FirstOrDefault();
                    if (cmd != null)
                        await ExecCommand(cmd);
                });
                return this._ExecCommandByLogicalID;
            }
        }

        /// <summary>
        /// Exécute une commande à partir de son "name"
        /// </summary>
        public RelayCommand<object> ExecCommandByName
        {
            get
            {
                this._ExecCommandByName = this._ExecCommandByName ?? new RelayCommand<object>(async parameters =>
                {
                    try
                    {
                        var cmd = Cmds.Where(c => c.Name.ToLower() == parameters.ToString().ToLower()).FirstOrDefault();
                        if (cmd != null)
                            await ExecCommand(cmd);
                    }
                    catch (Exception) { }
                });
                return this._ExecCommandByName;
            }
        }

        /// <summary>
        /// Exécute une commande à partir de son "generic_type"
        /// </summary>
        public RelayCommand<object> ExecCommandByType
        {
            get
            {
                this._ExecCommandByType = this._ExecCommandByType ?? new RelayCommand<object>(async parameters =>
                {
                    try
                    {
                        var cmd = Cmds.Where(c => c.Display.generic_type == parameters.ToString()).FirstOrDefault();
                        if (cmd != null)
                            await ExecCommand(cmd);
                    }
                    catch (Exception) { }
                });
                return this._ExecCommandByType;
            }
        }
    }
}